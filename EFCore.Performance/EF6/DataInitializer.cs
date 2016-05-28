using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Performance.EF6
{
    static class DataInitializer
    {
        public static void InitDatabase()
        {
            using (var context = new StarWarsContext())
            {
                if (context.People.Count() > 0)
                {
                    context.Database.ExecuteSqlCommand("Delete from dbo.Starships");
                    context.Database.ExecuteSqlCommand("Delete from dbo.People");
                }

                SeedData(context);
            }
        }

        public static void SeedData(StarWarsContext context)
        {
            context.People.AddRange( new List<Person> {
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
                }});

            context.SaveChanges();
        }

        internal static void LoadLotsOfData()
        {
            using (var context = new StarWarsContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                for (int i = 1; i < 10000; i++)
                {
                    context.People.Add(
                        new Person
                        {
                            Name = $"Person #{i}",
                        });
                }
                context.SaveChanges();
            }
        }
    }
}
