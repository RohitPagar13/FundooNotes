using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entities
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedOn { get; set; }= DateTime.Now;
        public bool isArchieve { get; set; } = false;
        public bool isTrashed { get; set; } = false;

        [Required]
        [NotNull]
        [ForeignKey("User")]
        public int userId { get; set; }
    }
}
