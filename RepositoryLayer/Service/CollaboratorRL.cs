using Microsoft.Data.SqlClient;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RepositoryLayer.Service
{
    public class CollaboratorRL : ICollaboratorRL
    {
        private readonly FundooContext _db;

        public CollaboratorRL(FundooContext db)
        {
            _db = db;
        }

        public Collaborator addCollaborator(CollaboratorModel collaboratormodel)
        {
            using(var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var checkcollaborator = _db.Collaborators.Where(c => c.Email.Equals(collaboratormodel.Email) && c.NoteId==collaboratormodel.NoteId).FirstOrDefault();
                    if (checkcollaborator != null)
                    {
                        throw new UserException("Collaborator already exists", "AlreadyExistsException");
                    }
                    Collaborator collaborator = new Collaborator()
                    {
                        Email = collaboratormodel.Email,
                        NoteId = collaboratormodel.NoteId,
                    };

                    _db.Collaborators.Add(collaborator);
                    _db.SaveChanges();
                    transaction.Commit();
                    return collaborator;
                }
                catch(SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        public List<Collaborator> getCollaboratorsByNoteId(int noteId)
        {
            try
            {
                var collaborators = _db.Collaborators.Where(c => c.NoteId == noteId);
                if (collaborators.Count() == 0)
                {
                    throw new UserException("No Collaborator found for the Note", "noCollaborator");
                }
                return collaborators.ToList();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public Collaborator removeCollaborator(string collaboratorEmail)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var collaborator = _db.Collaborators.FirstOrDefault(c=>c.Email.Equals(collaboratorEmail));
                    if(collaborator == null)
                    {
                        throw new UserException("Collaborator already removed or collaborator does not exists", "CollaboratorNotExistsException");
                    }
                    _db.Remove(collaborator);
                    _db.SaveChanges();
                    transaction.Commit();
                    return collaborator;
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
