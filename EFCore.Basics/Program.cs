using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Basics
{
    class Program
    {
        static void Main(string[] args)
        {
            using (StarWarsContext context = new StarWarsContext())
            {
                context.Database.EnsureCreated();

                Person luke = new Person
                {
                    Name = "Luke Skywalker",
                    HairColor = "Blond",
                    Height = 1.72
                };

                context.People.Add(luke);
                context.SaveChanges();

                var query = from p in context.People
                            where p.HairColor == "Blond"
                            select p;

                query.ToList().ForEach(p => Console.WriteLine(p.Name));
                
            }
        }
    }

    public class StarWarsContext : DbContext
    {        
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;database=EFCore.Basics;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HairColor { get; set; }
        public double Height { get; set; }
    }
}
