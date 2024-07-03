using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using RepositoryLayer.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NoteLabelBL:INoteLabelBL
    {
        private readonly INoteLabelRL noteLabelRL;

        public NoteLabelBL(INoteLabelRL noteLabelRL)
        {
            this.noteLabelRL = noteLabelRL;
        }

        public NoteLabel AddLabelToNote(NoteLabel nl)
        {
            try
            {
                return noteLabelRL.AddLabelToNote(nl);
            }
            catch
            {
                throw;
            }
        }

        public List<NoteLabelsDTO> getArchivedWithLabels(int userid)
        {
            try
            {
                return noteLabelRL.getArchivedWithLabels(userid);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Label>? GetLabelsFromNote(int NoteID)
        {
            try
            {
                return noteLabelRL.GetLabelsFromNote(NoteID);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<Note>? GetNotesFromLabel(int LabelID)
        {
            try
            {
                return noteLabelRL.GetNotesFromLabel(LabelID);
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<NoteLabelsDTO>> getNotesWithLabels(int userid)
        {
            try
            {
                return await noteLabelRL.getNotesWithLabels(userid);
            }
            catch
            {
                throw;
            }
        }

        public List<NoteLabelsDTO> getTrashedWithLabels(int userid)
        {
            try
            {
                return noteLabelRL.getTrashedWithLabels(userid);
            }
            catch
            {
                throw;
            }
        }

        public NoteLabel RemoveLabelFromNote(NoteLabel nl, int userid)
        {
            try
            {
                return noteLabelRL.RemoveLabelFromNote(nl, userid);
            }
            catch
            {
                throw;
            }
        }

        public NoteLabelsDTO getNoteWithLabelsByNoteId(int noteid, int userid)
        {
            try
            {
                return noteLabelRL.getNoteWithLabelsById(noteid, userid);
            }
            catch
            {
                throw;
            }
        }

    }
}
