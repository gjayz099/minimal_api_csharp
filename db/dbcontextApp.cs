using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using csharp.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace csharp.db
{
    public class dbcontextApp : IdentityDbContext
    {
        public  dbcontextApp(DbContextOptions<dbcontextApp> options) : base(options)
        {

        }

       public DbSet<Pokemon> Pokemons => Set<Pokemon>();

        internal async Task FindAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}