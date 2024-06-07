using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
<<<<<<< HEAD
    public class FundooContext: DbContext
=======
    public class FundooContext : DbContext
>>>>>>> HashConversion
    {
        public FundooContext(DbContextOptions options) : base(options) { }

        public DbSet<User> users { get; set; }
    }
}
