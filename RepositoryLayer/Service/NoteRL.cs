using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using RepositoryLayer.Utilities;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        private readonly FundooContext _db;
        private readonly IDistributedCache _cache;
        string cacheKey;

        public NoteRL(FundooContext _db, IDistributedCache cache)
        {
            this._db = _db;
            _cache = cache;
            cacheKey = $"{123}:GET_ALL_NOTES";
        }
        public NoteResponseModel addNote(NoteInputModel notemodel, int userId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Note n = new Note();
                    NoteResponseModel response = new NoteResponseModel();

                    n.Title=response.Title = notemodel.Title;
                    n.Description = response.Description= notemodel.Description;
                    n.userId=userId;

                    _db.notes.Add(n);
                    _db.SaveChanges();
                    transaction.Commit();
                    response.Id = n.Id;
                    response.CreatedOn = n.CreatedOn;


                    var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                    if (notesWithLabelsCache != null)
                    {
                        
                        notesWithLabelsCache.Add(new NoteLabelsDTO() { Id = n.Id, Title = n.Title, CreatedOn = n.CreatedOn, Description = n.Description, isTrashed = n.isTrashed, isArchieve = n.isArchieve, userId = n.userId, Labels = null });
                    }
                    CacheService.SetToCache(cacheKey, _cache, notesWithLabelsCache);

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


        public NoteResponseModel archived(int noteId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Note? n = _db.notes.Find(noteId);
                    if (n == null)
                    {
                        throw new UserException("Note with the specified ID does not exist.", "NoteNotFoundException");
                    }
                    else if (n.isTrashed)
                    {
                        throw new UserException("Unable to Archived as Note is deleted", "NoteArchievedException");
                    }
                    NoteResponseModel response = new NoteResponseModel();
                    response.Title = n.Title;
                    response.Description = n.Description;
                    response.Id = n.Id;
                    response.CreatedOn = n.CreatedOn;
                    n.isArchieve = !n.isArchieve;
                    _db.notes.Update(n);
                    _db.SaveChanges();
                    transaction.Commit();

                    var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                    if (notesWithLabelsCache != null)
                    {
                        foreach(var noteLabel in notesWithLabelsCache)
                        {
                            if(noteLabel.Id==noteId)
                            {
                                noteLabel.isArchieve=n.isArchieve;
                            }
                        }
                    }
                    CacheService.SetToCache(cacheKey, _cache, notesWithLabelsCache);

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

        //public NoteResponseModel getNoteById(int id)
        //{
        //    try
        //    {
        //        Note? note = _db.notes.Find(id);
        //        if (note == null)
        //        {
        //            throw new UserException("Note with the specified ID does not exist.", "NoteNotFoundException");
        //        }
        //        NoteResponseModel response = new NoteResponseModel();
        //        response.Title = note.Title;
        //        response.Description = note.Description;
        //        response.Id = note.Id;
        //        response.CreatedOn = note.CreatedOn;
        //        return response;
        //    }
        //    catch (SqlException se)
        //    {
        //        Console.WriteLine(se.ToString());
        //        throw;
        //    }
        //}

        //public List<NoteResponseModel> GetNotes(int userid)
        //{
        //    try
        //    {
        //        var result = _db.notes.Where(p=>p.userId == userid && !p.isTrashed && !p.isArchieve);
        //        List<NoteResponseModel>responseNotes = new List<NoteResponseModel>();
        //        foreach (var note in result)
        //        {
        //            NoteResponseModel noteResponse = new NoteResponseModel();
        //            noteResponse.Id = note.Id;
        //            noteResponse.Title = note.Title;
        //            noteResponse.Description = note.Description;
        //            noteResponse.CreatedOn = note.CreatedOn;

        //            responseNotes.Add(noteResponse);
        //        }
        //        return responseNotes;
        //    }
        //    catch (SqlException se)
        //    {
        //        Console.WriteLine(se.ToString());
        //        throw;
        //    }
        //}

        public NoteResponseModel removeNote(int id)
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
                    else if(n.isTrashed)
                    {
                        throw new UserException("Unable to delete Note as Note already deleted", "NoteDeletedException");
                    }
                    NoteResponseModel response = new NoteResponseModel();
                    response.Title = n.Title;
                    response.Description = n.Description;
                    response.Id = n.Id;
                    response.CreatedOn = n.CreatedOn;
                    _db.notes.Remove(n);
                    _db.SaveChanges();
                    transaction.Commit();

                    var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                    if (notesWithLabelsCache != null)
                    {
                        var removenote = notesWithLabelsCache.FirstOrDefault(nl=>nl.Id== id);
                        if (removenote == null)
                        {
                            return response;
                        }
                        notesWithLabelsCache.Remove(removenote);
                    }
                    CacheService.SetToCache(cacheKey, _cache, notesWithLabelsCache);

                    return response;
                }
                catch (SqlException se)
                {
                    Console.WriteLine(se.ToString());
                    throw;
                }
            }
        }

        public NoteResponseModel trashed(int noteId)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Note? n = _db.notes.Find(noteId);
                    if (n == null)
                    {
                        throw new UserException("Note with the specified ID does not exist.", "NoteNotFoundException");
                    } 
                    NoteResponseModel response = new NoteResponseModel();
                    response.Title = n.Title;
                    response.Description = n.Description;
                    response.Id = n.Id;
                    response.CreatedOn = n.CreatedOn;
                    n.isTrashed = !n.isTrashed;
                    _db.notes.Update(n);
                    _db.SaveChanges();
                    transaction.Commit();

                    var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                    if (notesWithLabelsCache != null)
                    {
                        foreach (var noteLabel in notesWithLabelsCache)
                        {
                            if (noteLabel.Id == noteId)
                            {
                                noteLabel.isArchieve = n.isTrashed;
                            }
                        }
                    }
                    CacheService.SetToCache(cacheKey, _cache, notesWithLabelsCache);

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

        public NoteResponseModel updateNoteById(int id, NoteInputModel notemodel)
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
                    else if (n.isTrashed)
                    {
                        throw new UserException("Unable to Update Note as Note is deleted", "NoteDeletedException");
                    }
                    NoteResponseModel response = new NoteResponseModel();
                    n.Title = response.Title = notemodel.Title;
                    n.Description = response.Description = notemodel.Description;
                    _db.notes.Update(n);
                    _db.SaveChanges();
                    transaction.Commit();
                    response.Id = n.Id;
                    response.CreatedOn = n.CreatedOn;

                    var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                    if (notesWithLabelsCache != null)
                    {
                        foreach (var noteLabel in notesWithLabelsCache)
                        {
                            if (noteLabel.Id == id)
                            {
                                noteLabel.Title = n.Title;
                                noteLabel.Description = n.Description;
                                noteLabel.Id = n.Id;
                                noteLabel.CreatedOn = n.CreatedOn;
                                noteLabel.isTrashed = n.isTrashed;
                                noteLabel.isArchieve = n.isArchieve;
                                noteLabel.userId = n.userId;
                                noteLabel.Labels = null;
                            }
                        }
                    }
                    CacheService.SetToCache(cacheKey, _cache, notesWithLabelsCache);

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

        //public List<NoteResponseModel> getTrashed(int userid)
        //{
        //    try
        //    {
        //        var result = _db.notes.Where(p => p.userId == userid && p.isTrashed==true).ToList();
        //        if (!result.Any())
        //        {
        //            throw new UserException("No trashed notes found", "EmptyTrashException");
        //        }
        //        List<NoteResponseModel> responseNotes = new List<NoteResponseModel>();
        //        foreach (var note in result)
        //        {
        //            NoteResponseModel noteResponse = new NoteResponseModel();
        //            noteResponse.Id = note.Id;
        //            noteResponse.Title = note.Title;
        //            noteResponse.Description = note.Description;
        //            noteResponse.CreatedOn = note.CreatedOn;

        //            responseNotes.Add(noteResponse);
        //        }
        //        return responseNotes;
        //    }
        //    catch (SqlException se)
        //    {
        //        Console.WriteLine(se.ToString());
        //        throw;
        //    }
        //}

        //public List<NoteResponseModel> getArchived(int userid)
        //{
        //    try
        //    {
        //        var result = _db.notes.Where(p => p.userId == userid && p.isArchieve==true && p.isTrashed==false).ToList();
        //        if (!result.Any())
        //        {
        //            throw new UserException("No archived notes found", "EmptyArchiveException");
        //        }
        //        List<NoteResponseModel> responseNotes = new List<NoteResponseModel>();
        //        foreach (var note in result)
        //        {
        //            NoteResponseModel noteResponse = new NoteResponseModel();
        //            noteResponse.Id = note.Id;
        //            noteResponse.Title = note.Title;
        //            noteResponse.Description = note.Description;
        //            noteResponse.CreatedOn = note.CreatedOn;

        //            responseNotes.Add(noteResponse);
        //        }
        //        return responseNotes;
        //    }
        //    catch (SqlException se)
        //    {
        //        Console.WriteLine(se.ToString());
        //        throw;
        //    }
        //}
    }
}
