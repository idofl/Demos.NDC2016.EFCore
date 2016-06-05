using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Queries.EFCore
{
    static class DataInitializer
    {
        public static void InitDatabase(DbContextOptions<EFCore.StarWarsContext> options)
        {
            using (var context = new StarWarsContext(options))
            {
                context.Database.EnsureCreated();

                if (context.People.Any())
                {
                    context.Database.ExecuteSqlCommand("Delete from dbo.Starship");
                    context.Database.ExecuteSqlCommand("Delete from dbo.People");
                }

                SeedData(context);
            }
        }

        public static void SeedData(StarWarsContext context)
        {
            context.People.AddRange(
                new Person
                {
                    Name = "Luke Skywalker",
                    HairColor = "Blond",
                    Height = 1.72,
                    Starships = new List<Starship>
                    {
                        new Starship
                        {
                            Name = "X-Wing",
                            Cost = 10000,
                            MaxPassengers = 1,
                        }
                    }
                },
                new Person
                {
                    Name = "Han Solo",
                    HairColor = "Brown",
                    Height = 1.8,
                    Starships = new List<Starship>
                    {
                        new Starship
                        {
                            Name = "Millennium Falcon",
                            Cost = 100000,
                            MaxPassengers = 6,
                        }
                    }
                });

            context.SaveChanges();
        }    
    }
}