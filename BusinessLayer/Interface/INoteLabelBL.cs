using ModelLayer;
using RepositoryLayer.Entities;
using RepositoryLayer.Utilities.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INoteLabelBL
    {
        public NoteLabel AddLabelToNote(NoteLabel nl);

        public IEnumerable<Label>? GetLabelsFromNote(int NoteID);

        public IEnumerable<Note>? GetNotesFromLabel(int LabelID);

        public NoteLabel RemoveLabelFromNote(NoteLabel nl);

        public Task<List<NoteLabelsDTO>> getNotesWithLabels(int userid);

        public List<NoteLabelsDTO> getArchivedWithLabels(int userid);

        public List<NoteLabelsDTO> getTrashedWithLabels(int userid);
        public NoteLabelsDTO getNoteWithLabelsByNoteId(int id);
    }
}
