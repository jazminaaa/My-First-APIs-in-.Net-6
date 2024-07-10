using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore; //or use (found in stackoverflow):Microsoft.EntityFrameworkCore.Tools


namespace MoviesApi.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplictationDbContext(DbContextOptions<ApplictationDbContext> options) : base(options)

        { 

        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movies> Movies { get; set; }


    }
}

