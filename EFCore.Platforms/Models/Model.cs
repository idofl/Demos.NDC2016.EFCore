using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Platforms.Models
{
    public class StarWarsContext : DbContext
    {
        public StarWarsContext(DbContextOptions<StarWarsContext> options) :base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasAlternateKey(p => p.SwapiUrl);

            // Alternate key can be used in related entities as FK
            // .HasForeignKey(related => related.ParentKey)
            // .HasPrincipalKey(parent => parent.AltKey);

        }
        public DbSet<Person> People { get; set; }
        public DbSet<Starship> Starships { get; set; }     
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HairColor { get; set; }
        public double Height { get; set; }
        public string SwapiUrl { get; set; }
        public List<Starship> Starships { get; set; }
    }

    public class Starship
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int MaxPassengers { get; set; }
        public Person Owner { get; set; }
    }
}
