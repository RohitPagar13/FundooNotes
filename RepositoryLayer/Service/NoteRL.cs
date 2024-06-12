using Azure;
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
        public NoteResponseModel addNote(NoteInputModel note, int userId)
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
                    n.userId=userId;
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
            try
            {
                Note? note = _db.notes.Find(id);
                if (note == null)
                {
                    throw new UserException("Note with the specified ID does not exist.", "NoteNotFoundException");
                }
                NoteResponseModel response = new NoteResponseModel();
                response.Title = note.Title;
                response.Description = note.Description;
                response.Id = note.Id;
                response.CreatedOn = note.CreatedOn;
                return response;
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }

        public List<NoteResponseModel> GetNotes(int userid)
        {
            try
            {
                var result = _db.notes.Where(p=>p.userId == userid);
                List<NoteResponseModel>responseNotes = new List<NoteResponseModel>();
                foreach (var note in result)
                {
                    NoteResponseModel noteResponse = new NoteResponseModel();
                    noteResponse.Id = note.Id;
                    noteResponse.Title = note.Title;
                    noteResponse.Description = note.Description;
                    noteResponse.CreatedOn = note.CreatedOn;

                    responseNotes.Add(noteResponse);
                }
                return responseNotes;
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }

        public NoteResponseModel removeNote(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Note? note = _db.notes.Find(id);
                    if (note == null)
                    {
                        throw new UserException("Note with the specified ID does not exist.", "CustomerNotFoundException");
                    }
                    NoteResponseModel response = new NoteResponseModel();
                    response.Title = note.Title;
                    response.Description = note.Description;
                    response.Id = note.Id;
                    response.CreatedOn = note.CreatedOn;
                    _db.notes.Remove(note);
                    _db.SaveChanges();
                    transaction.Commit();
                    return response;
                }
                catch (SqlException se)
                {
                    Console.WriteLine(se.ToString());
                    throw;
                }
            }
        }

        public NoteResponseModel updateNoteById(int id, NoteInputModel note)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Note? n = _db.notes.Find(id);
                    if (n == null)
                    {
                        throw new UserException("Note with the specified ID does not exist.", "NoteNotFoundException");
                    }
                    NoteResponseModel response = new NoteResponseModel();
                    n.Title = response.Title = note.Title;
                    n.Description = response.Description = note.Description;
                    _db.notes.Update(n);
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
    }
}
