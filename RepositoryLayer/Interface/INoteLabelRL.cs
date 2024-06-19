using ModelLayer;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteLabelRL
    {
        public NoteLabel AddLabelToNote(NoteLabel nl);

        public IEnumerable<Label>? GetLabelsFromNote(int NoteID);

        public IEnumerable<Note>? GetNotesFromLabel(int LabelID);

        public NoteLabel RemoveLabelFromNote(NoteLabel nl);
        public Dictionary<Note, List<Label>> getNotesWithLabels(int userid);
    }
}
