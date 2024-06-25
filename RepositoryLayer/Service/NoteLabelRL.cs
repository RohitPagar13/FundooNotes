using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Utilities.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteLabelRL:INoteLabelRL
    {
        private readonly FundooContext _db;

        public NoteLabelRL(FundooContext _db)
        {
            this._db = _db;
        }

        public NoteLabel AddLabelToNote(NoteLabel nl)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var note = _db.notes.FirstOrDefault(n => n.Id == nl.noteId);
                    var label = _db.labels.FirstOrDefault(l => l.Id == nl.labelId);

                    if (note == null || label == null)
                    {
                        throw new UserException("incorrect noteId or labelId", "IncorrectDataException");
                    }

                    NoteLabel noteLabel = new NoteLabel()
                    {
                        noteId = nl.noteId,
                        labelId = nl.labelId
                    };

                    _db.NoteLabels.Add(noteLabel);
                    _db.SaveChanges();
                    transaction.Commit();
                    return noteLabel;
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

        public NoteLabel RemoveLabelFromNote(NoteLabel nl)
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

        public List<NoteLabelsDTO> getNotesWithLabels(int userid)
        {
            try
            {
                List<Note> notes = _db.notes.Where(note=>note.userId==userid && note.isTrashed==false && note.isArchieve==false).ToList();

                if (notes == null || notes.Count == 0)
                {
                    Console.WriteLine("No notes found for user with ID: " + userid);
                    return new List<NoteLabelsDTO>();
                }

                var notesWithLabels = new List<NoteLabelsDTO>();
                foreach(var note in notes)
                {
                    var labels = GetLabelsFromNote(note.Id)?.ToList()??new List<Label>();

                    if (labels == null || labels.Count == 0)
                    {
                        Console.WriteLine("No labels found for note with ID: " + note.Id);
                    }

                    //var labels = new LabelRL(_db).GetLabels().ToList();
                    notesWithLabels.Add(new NoteLabelsDTO { Id = note.Id,Title=note.Title, CreatedOn=note.CreatedOn,Description=note.Description, Labels=labels }) ;
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
                var notes = _db.notes.Where(p => p.userId == userid && p.isTrashed == true).ToList();
                if (notes == null || notes.Count == 0)
                {
                    Console.WriteLine("No notes found for user with ID: " + userid);
                    return new List<NoteLabelsDTO>();
                }
                var notesWithLabels = new List<NoteLabelsDTO>();
                foreach (var note in notes)
                {
                    var labels = GetLabelsFromNote(note.Id)?.ToList() ?? new List<Label>();

                    if (labels == null || labels.Count == 0)
                    {
                        Console.WriteLine("No labels found for note with ID: " + note.Id);
                    }

                    //var labels = new LabelRL(_db).GetLabels().ToList();
                    notesWithLabels.Add(new NoteLabelsDTO { Id = note.Id, Title = note.Title, CreatedOn = note.CreatedOn, Description = note.Description, Labels = labels });
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
                var notes = _db.notes.Where(p => p.userId == userid && p.isArchieve == true && p.isTrashed == false).ToList();
                if (notes == null || notes.Count == 0)
                {
                    Console.WriteLine("No notes found for user with ID: " + userid);
                    return new List<NoteLabelsDTO>();
                }
                var notesWithLabels = new List<NoteLabelsDTO>();
                foreach (var note in notes)
                {
                    var labels = GetLabelsFromNote(note.Id)?.ToList() ?? new List<Label>();

                    if (labels == null || labels.Count == 0)
                    {
                        Console.WriteLine("No labels found for note with ID: " + note.Id);
                    }

                    //var labels = new LabelRL(_db).GetLabels().ToList();
                    notesWithLabels.Add(new NoteLabelsDTO { Id = note.Id, Title = note.Title, CreatedOn = note.CreatedOn, Description = note.Description, Labels = labels });
                }
                return notesWithLabels;
            }
            catch (SqlException se)
            {
                Console.WriteLine(se.ToString());
                throw;
            }
        }

        public NoteLabelsDTO getNoteWithLabelsById(int id)
        {
            try
            {
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
