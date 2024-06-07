using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class LoginResponse
    {
        public int ID {  get; set; }
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string BirthDate { get; set; }

    }
}
