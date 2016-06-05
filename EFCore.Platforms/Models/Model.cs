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

        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
          //  optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;database=EFCore.Queries.Core;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HairColor { get; set; }
        public double Height { get; set; }
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
