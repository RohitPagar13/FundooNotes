using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        private readonly FundooContext _db;

        public NoteRL(FundooContext db)
        {
            this._db = db;
        }
        public NoteResponseModel addNote(NoteInputModel note)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    if (_db.notes.Any(n => n.Title.Equals(note.Title)))
                    {
                        throw new UserException("Note with specified Title Already Exists", "NoteAlreadyExists");
                    }
                    Note n = new Note();
                    NoteResponseModel response = new NoteResponseModel();

                    n.Title=response.Title = note.Title;
                    n.Description = response.Description= note.Description;

                    _db.notes.Add(n);
                    _db.SaveChanges();
                    transaction.Commit();
                    response.Id = n.Id;
                    response.CreatedOn = n.CreatedOn;
                    return response;
                }
                catch (SqlException se)
                {
                    transaction.Rollback();
                    Console.WriteLine(se.ToString());
                    throw;
                }
            }
        }

        public NoteResponseModel getNoteById(int id)
        {
            throw new NotImplementedException();
        }

        public List<Note> GetNotes(int id)
        {
            throw new NotImplementedException();
        }

        public NoteResponseModel removeNote(int id)
        {
            throw new NotImplementedException();
        }

        public NoteResponseModel updateNoteById(int id, NoteInputModel note)
        {
            throw new NotImplementedException();
        }
    }
}
