using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
                var matchLabelIds = _db.NoteLabels.Where(n=>n.noteId==NoteID).Select(l=>l.labelId);

                if (matchLabelIds.Count()==0)
                {

                    throw new UserException("No labels for the Note or NoteID is wrong", "NoLabelsException");
                }


                var labels = _db.labels.Where(l=>matchLabelIds.Contains(l.Id));
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

                if (matchNoteIds == null)
                {
                    throw new UserException("No Note for the Label", "NoNoteException");
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

                    _db.NoteLabels.Remove(noteLabel);
                    _db.SaveChanges();
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
    }
}
