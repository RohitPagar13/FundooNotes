using BusinessLayer.Interface;
using ModelLayer;
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
    public class CollaboratorBL:ICollaboratorBL
    {
        private readonly ICollaboratorRL collaboratorRL;

        public CollaboratorBL(ICollaboratorRL collaboratorRL)
        {
            this.collaboratorRL = collaboratorRL;
        }

        public Collaborator addCollaborator(CollaboratorModel collaborator)
        {
            try
            {
                return collaboratorRL.addCollaborator(collaborator);
            }
            catch
            {
                throw;
            }
        }

        public List<Collaborator> getCollaboratorsByNoteId(int noteId)
        {
            try
            {
                return collaboratorRL.getCollaboratorsByNoteId(noteId);
            }
            catch
            {
                throw;
            }
        }

        public Collaborator removeCollaborator(string collaboratorEmail)
        {
            try
            {
                return collaboratorRL.removeCollaborator(collaboratorEmail);
            }
            catch
            {
                throw;
            }
        }
    }
}
