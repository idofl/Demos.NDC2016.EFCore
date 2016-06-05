using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Platforms.Models
{
    public static class ContextExtensions
    {
        public static void EnsureSeedData(this StarWarsContext context)
        {
            if (!context.People.Any())
            {
                context.People.AddRange(
                   new Person
                   {
                       Name = "Luke Skywalker",
                       HairColor = "Blond",
                       Height = 1.72,
                       SwapiUrl = "http://swapi.co/api/people/1/",
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
                       SwapiUrl = "http://swapi.co/api/people/14/",
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
}
