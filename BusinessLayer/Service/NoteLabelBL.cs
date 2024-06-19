using BusinessLayer.Interface;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
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

        public Dictionary<Note, List<Label>> getNotesWithLabels(int userid)
        {
            try
            {
                return noteLabelRL.getNotesWithLabels(userid);
            }
            catch
            {
                throw;
            }
        }

        public NoteLabel RemoveLabelFromNote(NoteLabel nl)
        {
            try
            {
                return noteLabelRL.RemoveLabelFromNote(nl);
            }
            catch
            {
                throw;
            }
        }
    }
}
