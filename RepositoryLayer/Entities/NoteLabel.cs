using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class NoteLabel
    {
        [ForeignKey("Note")]
        public int noteId {  get; set; }

        [ForeignKey("Label")]
        public int labelId { get; set; }
    }
}
