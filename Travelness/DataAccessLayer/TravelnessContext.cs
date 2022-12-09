using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TravelnessContext : DbContext
    {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Sightseeing> Sightseeings { get; set; }
        public DbSet<Tour> Tours { get; set; }

        public TravelnessContext(DbContextOptions<TravelnessContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sightseeing>().HasMany(x => x.Rates).WithOne(x => x.Sightseeing).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
