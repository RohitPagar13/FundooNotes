using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class CollaboratorModel
    {
        [Required]
        [NotNull]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int NoteId { get; set; }
    }
}
