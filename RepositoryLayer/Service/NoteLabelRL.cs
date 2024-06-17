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
                var matchLabelNote = _db.notes.Include(n => n.noteLables).FirstOrDefault(n => n.Id == NoteID);

                if (matchLabelNote == null)
                {

                    throw new UserException("Incorrect noteId or note not available", "IncorrectDataException");
                }

                var labels = matchLabelNote.noteLables?.ToList();

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
                var matchNoteByLabel = _db.labels.Include(l => l.labelNotes).FirstOrDefault(l => l.Id == LabelID);

                if (matchNoteByLabel == null)
                {
                    throw new UserException("Incorrect labelId or label not available", "IncorrectDataException");
                }

                var notes = matchNoteByLabel.labelNotes?.ToList();

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
