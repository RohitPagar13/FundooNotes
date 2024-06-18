using ModelLayer;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface ICollaboratorRL
    {
        public Collaborator addCollaborator(CollaboratorModel collaborator);
        public Collaborator removeCollaborator(string collaboratorEmail);
        public List<Collaborator> getCollaboratorsByNoteId(int noteId);
    }
}
