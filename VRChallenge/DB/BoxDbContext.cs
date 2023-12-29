using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRChallenge.DB.Model;

namespace VRChallenge.DB
{
    public class BoxDbContext : DbContext
    {
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Item> Items { get; set; }

        private readonly string _tableName;

        public BoxDbContext(string tableName)
        {
            _tableName = tableName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Configure your database connection here
            optionsBuilder.UseInMemoryDatabase(_tableName);
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<Box>()
                .HasKey(sc => sc.Id);

            modelBuilder.Entity<Item>()
                .HasKey(sc => sc.Id);

            modelBuilder.Entity<Box>()
                .HasMany(sc => sc.Items);
        }

    }
}
