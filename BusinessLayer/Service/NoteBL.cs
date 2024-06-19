using BusinessLayer.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using RepositoryLayer.CustomException;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NoteBL : INoteBL
    {
        private readonly INoteRL noteRL;

        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }
        public NoteResponseModel addNote(NoteInputModel notemodel, int userId)
        {
            try
            {
                var note = noteRL.addNote(notemodel, userId);
                if (note != null)
                {
                    return note;
                }
                else { throw new UserException("Note not found", "NoteNotFoundException"); } 
            }
            catch
            {
                throw;
            }
        }

        public NoteResponseModel archived(int noteId)
        {
            try
            {
                return noteRL.archived(noteId);
            }
            catch
            {
                throw;
            }
        }

        public List<NoteResponseModel> getArchived(int userid)
        {
            try
            {
                return noteRL.getArchived(userid);
            }
            catch
            {
                throw;
            }
        }

        public NoteResponseModel getNoteById(int id)
        {
            try
            {
                return noteRL.getNoteById(id);
            }
            catch
            {
                throw;
            }
        }

        public List<NoteResponseModel> GetNotes(int userid)
        {
            try
            {
                return noteRL.GetNotes(userid);
            }
            catch
            {
                throw;
            }
        }

        public List<NoteResponseModel> getTrashed(int userid)
        {
            try
            {
                return noteRL.getTrashed(userid);
            }
            catch
            {
                throw;
            }
        }

        public NoteResponseModel removeNote(int id)
        {
            try
            {
                return noteRL.removeNote(id);
            }
            catch
            {
                throw;
            }
        }

        public NoteResponseModel trashed(int noteId)
        {
            try
            {
                return noteRL.trashed(noteId);
            }
            catch
            {
                throw;
            }
        }

        public NoteResponseModel updateNoteById(int id, NoteInputModel note)
        {
            try
            {
                return noteRL.updateNoteById(id, note);
            }
            catch
            {
                throw;
            }
        }
    }
}
