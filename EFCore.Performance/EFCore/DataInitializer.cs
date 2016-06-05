using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Performance.EFCore
{
    static class DataInitializer
    {
        public static void Warmup(bool clearData, DbContextOptions<EFCore.StarWarsContext> options)
        {
            using (var context = new StarWarsContext(options))
            {
                context.Database.EnsureCreated();
                context.People.FirstOrDefault();

                if (clearData && context.People.Any())
                {
                    context.Database.ExecuteSqlCommand("delete dbo.Starships");
                    context.Database.ExecuteSqlCommand("delete dbo.People");
                }                
            }
        }

        internal static void LoadLotsOfData(DbContextOptions<EFCore.StarWarsContext> options)
        {
            using (var context = new StarWarsContext(options))
            {
                for (int i = 1; i < Program.TABLE_SIZE; i++)
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