﻿using ModelLayer;
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
        public List<Note> GetNotes(int id);

        public NoteResponseModel getNoteById(int id);

        public NoteResponseModel updateNoteById(int id, NoteInputModel note);

        public NoteResponseModel addNote(NoteInputModel note);

        public NoteResponseModel removeNote(int id);
    }
}