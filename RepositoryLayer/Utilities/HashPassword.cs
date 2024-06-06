using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Utilities
{
    public static class HashPassword
    {
        public static string convertToHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        public static bool verifyHash(string password, string hashPass)
        {
            return BCrypt.Net.BCrypt.Verify(password,hashPass);

        }
    }
}
