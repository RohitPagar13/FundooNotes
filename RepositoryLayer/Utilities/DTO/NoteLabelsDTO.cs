using ModelLayer;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Utilities.DTO
{
    public class NoteLabelsDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public bool isArchieve { get; set; } = false;
        public bool isTrashed { get; set; } = false;

        public int userId {  get; set; }

        public List<Label>? Labels { get; set; }
    }
}
