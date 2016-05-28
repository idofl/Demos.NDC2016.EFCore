using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Performance
{
    class Program
    {
        static void Main(string[] args)
        {

          

            //CompareFetch();

        }

     

        private static void CompareFetch()
        {
            Console.WriteLine("Populating database with 10000 people");
            EF6.DataInitializer.LoadLotsOfData();
            EFCore.DataInitializer.LoadLotsOfData();

            CompareSimpleQuery();
        }

        private static void CompareSimpleQuery()
        {
            Stopwatch watch = new Stopwatch();

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($" Iteration {i}");
                watch.Restart();
                using (var context = new EF6.StarWarsContext())
                {
                    context.People.ToList();
                }
                watch.Stop();
                var prevResult = watch.ElapsedMilliseconds;
                Console.WriteLine($"  - EF6.x:      {watch.ElapsedMilliseconds}ms");

                watch.Restart();
                using (var context = new EFCore.StarWarsContext())
                {
                    context.People.ToList();
                }
                watch.Stop();
                Console.WriteLine($"  - EF Core:    {watch.ElapsedMilliseconds}ms");

                var improvement = (prevResult - watch.ElapsedMilliseconds) / (double)prevResult;
                Console.WriteLine($"  - Improvement:{improvement.ToString("P0")}");
                Console.WriteLine();
            }

        }

    
    }
}
