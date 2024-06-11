using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class NoteInputModel
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
