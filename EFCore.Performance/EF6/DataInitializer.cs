using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Performance.EF6
{
    static class DataInitializer
    {
        public static void Warmup(bool clearData)
        {
            using (var context = new StarWarsContext())
            {
                context.People.FirstOrDefault();
                if (clearData && context.People.Any())
                {
                    context.Database.ExecuteSqlCommand("delete dbo.Starships");
                    context.Database.ExecuteSqlCommand("delete dbo.People");
                }              
            }
        }

        internal static void LoadLotsOfData()
        {
            using (var context = new StarWarsContext())
            {
                context.Configuration.AutoDetectChangesEnabled = false;
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
