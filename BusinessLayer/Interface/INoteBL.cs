using ModelLayer;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INoteBL
    {
        public List<NoteResponseModel> GetNotes(int id);

        public NoteResponseModel getNoteById(int id);

        public NoteResponseModel updateNoteById(int id, NoteInputModel note);

        public NoteResponseModel addNote(NoteInputModel note, int userId);

        public NoteResponseModel removeNote(int id);

        public NoteResponseModel archived(int noteId);

        public NoteResponseModel trashed(int noteId);

        public List<NoteResponseModel> getTrashed(int userid);

        public List<NoteResponseModel> getArchived(int userid);
    }
}
