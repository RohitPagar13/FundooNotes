using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Utilities;
using RepositoryLayer.Utilities.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteLabelRL:INoteLabelRL
    {
        private readonly FundooContext _db;
        private readonly IDistributedCache _cache;
        string cacheKey; 
        public NoteLabelRL(FundooContext _db, IDistributedCache cache)
        {
            this._db = _db;
            _cache = cache;
        }

        public NoteLabel AddLabelToNote(NoteLabel nl)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                     
                    var noteforId = _db.notes.FirstOrDefault(n => n.Id == nl.noteId);
                    var labelcheck = _db.labels.FirstOrDefault(l => l.Id == nl.labelId);
                    
                    if (noteforId == null || labelcheck == null)
                    {
                        throw new UserException("incorrect noteId or labelId", "IncorrectDataException");
                    }

                    NoteLabel noteLabelset = new NoteLabel()
                    {
                        noteId = nl.noteId,
                        labelId = nl.labelId
                    };

                    _db.NoteLabels.Add(noteLabelset);
                    _db.SaveChanges();
                    transaction.Commit();

                    cacheKey = noteforId.userId.ToString();
                    var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                    if (notesWithLabelsCache != null)
                    {

                        foreach (var noteLabel in notesWithLabelsCache)
                        {
                            if (noteLabel.Id == nl.noteId)
                            {
                                var label = _db.labels.FirstOrDefault(l => l.Id == nl.labelId);
                                if (label == null)
                                {
                                    throw new UserException("No specified Label found", "LabelNotFoundException");
                                }
                                noteLabel.Labels?.Add(label);
                            }
                        }
                    }
                    CacheService.SetToCache(cacheKey, _cache, notesWithLabelsCache);
                    return noteLabelset;
                    
                }
                catch (SqlException se)
                {
                    transaction.Rollback();
                    Console.WriteLine(se.ToString());
                    throw;
                }
            }
        }

        public IEnumerable<Label>? GetLabelsFromNote(int NoteID)
        {
            try
            {
                var matchLabelIds = _db.NoteLabels.Where(n=>n.noteId==NoteID).Select(l=>l.labelId).ToList();

                if (!matchLabelIds.Any())
                {
                    return Enumerable.Empty<Label>();
                }

                var labels = _db.labels.Where(l=>matchLabelIds.Contains(l.Id)).ToList();
                return labels;
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }

        public IEnumerable<Note>? GetNotesFromLabel(int LabelID)
        {
            try
            {
                var matchNoteIds = _db.NoteLabels.Where(l=>l.labelId==LabelID).Select(n=>n.noteId);


                if (!matchNoteIds.Any())
                {
                    throw new UserException("No Notes for the Label or LabelID is wrong", "NoNoteException");
                }
                var notes = _db.notes.Where(n=>matchNoteIds.Contains(n.Id));

                return notes;
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }

        public NoteLabel RemoveLabelFromNote(NoteLabel nl,int userId)
        {

            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var notelabel = _db.NoteLabels.FirstOrDefault(n=>n.noteId==nl.noteId && n.labelId==nl.labelId);

                    if (notelabel==null)
                    {
                        throw new UserException("incorrect noteId or labelId", "IncorrectDataException");
                    }

                    _db.NoteLabels.Remove(notelabel);
                    _db.SaveChanges();
                    transaction.Commit();

                    cacheKey = userId.ToString();
                    var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                    if (notesWithLabelsCache != null)
                    {

                        foreach(var noteLabel in notesWithLabelsCache)
                        {
                            if (noteLabel.Id == nl.noteId)
                            {
                                var label = noteLabel.Labels?.FirstOrDefault(l=>l.Id==nl.labelId);
                                if (label == null) 
                                { 
                                    throw new UserException("No specified Label found", "LabelNotFoundException");
                                }
                                noteLabel.Labels?.Remove(label);
                            }
                        }
                    }
                    CacheService.SetToCache(cacheKey, _cache, notesWithLabelsCache);
                    return notelabel;
                }
                catch (SqlException se)
                {
                    transaction.Rollback();
                    Console.WriteLine(se.ToString());
                    throw;
                }
            }
        }

        public async Task<List<NoteLabelsDTO>> getNotesWithLabels(int userid)
        {
            try
            {
                cacheKey = userid.ToString();
                var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                
                if(notesWithLabelsCache == null)
                {
                    List<Note> notesall = await _db.notes.Where(note => note.userId == userid).ToListAsync();

                    if (notesall == null || notesall.Count == 0)
                    {
                        throw new UserException("Notes not found", "NotesNotFound");
                    }
                    notesWithLabelsCache = new List<NoteLabelsDTO>();
                    foreach (var note in notesall)
                    {
                        var labels = GetLabelsFromNote(note.Id)?.ToList() ?? new List<Label>();

                        notesWithLabelsCache?.Add(new NoteLabelsDTO(){ Id = note.Id, Title = note.Title, CreatedOn = note.CreatedOn, Description = note.Description, isTrashed = note.isTrashed, isArchieve = note.isArchieve,userId=userid, Labels = labels });
                    }
                    CacheService.SetToCache(cacheKey, _cache, notesWithLabelsCache);
                }

                List<NoteLabelsDTO>? notesWithLabels = notesWithLabelsCache?.Where(nl => nl.isTrashed == false && nl.isArchieve == false && nl.userId==userid).ToList();
                if (notesWithLabels == null || notesWithLabels.Count == 0)
                {
                    throw new UserException("Notes not found", "NotesNotFound");
                }
                return notesWithLabels;
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }

        public List<NoteLabelsDTO> getTrashedWithLabels(int userid)
        {
            try
            {
                cacheKey = userid.ToString();
                var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                if (notesWithLabelsCache != null)
                {
                    List<NoteLabelsDTO>? notesWithLabelscache = notesWithLabelsCache?.Where(nl => nl.userId == userid && nl.isTrashed == true).ToList();
                    if (notesWithLabelscache == null || notesWithLabelscache.Count == 0)
                    {
                        throw new UserException("No trashed notes found", "TrashNotFound");
                    }
                    return notesWithLabelscache;
                }
                    var notes = _db.notes.Where(p => p.userId == userid && p.isTrashed == true).ToList();
                    if (notes == null || notes.Count == 0)
                    {
                        throw new UserException("No trashed notes found", "TrashNotFound");
                    }
                    var notesWithLabels = new List<NoteLabelsDTO>();
                    foreach (var note in notes)
                    {
                        var labels = GetLabelsFromNote(note.Id)?.ToList() ?? new List<Label>();

                        notesWithLabels.Add(new NoteLabelsDTO { Id = note.Id, Title = note.Title, CreatedOn = note.CreatedOn, Description = note.Description,userId=userid, isTrashed = note.isTrashed, isArchieve = note.isArchieve, Labels = labels });
                    }
                    return notesWithLabels;
                
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }

        public List<NoteLabelsDTO> getArchivedWithLabels(int userid)
        {
            try
            {
                cacheKey = userid.ToString();
                var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                if (notesWithLabelsCache != null)
                {
                    List<NoteLabelsDTO>? notesWithLabelscache = notesWithLabelsCache?.Where(nl => nl.userId == userid && nl.isArchieve == true && nl.isTrashed == false).ToList();
                    if (notesWithLabelscache == null || notesWithLabelscache.Count == 0)
                    {
                        throw new UserException("No archived notes found", "ArchiveNotFound");
                    }
                    return notesWithLabelscache;
                }
                var notes = _db.notes.Where(p => p.userId == userid && p.isArchieve == true && p.isTrashed == false).ToList();
                if (notes == null || notes.Count == 0)
                {
                    throw new UserException("No archived notes found","ArchiveNotFound");
                }
                var notesWithLabels = new List<NoteLabelsDTO>();
                foreach (var note in notes)
                {
                    var labels = GetLabelsFromNote(note.Id)?.ToList() ?? new List<Label>();

                    notesWithLabels.Add(new NoteLabelsDTO { Id = note.Id, Title = note.Title, CreatedOn = note.CreatedOn, Description = note.Description,userId=userid, isTrashed = note.isTrashed, isArchieve = note.isArchieve, Labels = labels });
                }
                return notesWithLabels;
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }

        public NoteLabelsDTO getNoteWithLabelsById(int id, int userid)
        {
            try
            {
                cacheKey = userid.ToString();
                var notesWithLabelsCache = CacheService.GetFromCache<List<NoteLabelsDTO>>(cacheKey, _cache);
                if (notesWithLabelsCache != null)
                {
                    var notesWithLabelscache = notesWithLabelsCache.FirstOrDefault(nl=>nl.Id==id);
                    if(notesWithLabelscache == null)
                    {
                        throw new UserException ("Note with the specified ID does not exist.", "NoteNotFoundException");
                    }
                    var noteWithLabels = new NoteLabelsDTO { Id = notesWithLabelscache.Id, Title = notesWithLabelscache.Title, CreatedOn = notesWithLabelscache.CreatedOn, Description = notesWithLabelscache.Description, Labels = notesWithLabelscache.Labels };
                    return notesWithLabelscache;
                }

                Note? note = _db.notes.Find(id);
                if (note == null)
                {
                    throw new UserException("Note with the specified ID does not exist.", "NoteNotFoundException");
                }
                var labels = GetLabelsFromNote(note.Id)?.ToList() ?? new List<Label>();
                return new NoteLabelsDTO { Id = note.Id, Title = note.Title, CreatedOn = note.CreatedOn, Description = note.Description, Labels = labels };
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }
    }
}
