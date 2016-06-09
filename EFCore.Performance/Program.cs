using Microsoft.EntityFrameworkCore;
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
        static DbContextOptions<EFCore.StarWarsContext> _options;
        public const int TABLE_SIZE = 10000;

        static void Main(string[] args)
        {
            DbContextOptionsBuilder<EFCore.StarWarsContext> optionsBuilder;
            optionsBuilder = new DbContextOptionsBuilder<EFCore.StarWarsContext>();
            optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;database=EFCore.Performance.Core;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            _options = optionsBuilder.Options;

            Warmup(true);
            CompareSave();

            //Warmup(false);
            //CompareFetch();

        }

        private static void Warmup(bool clearData)
        {
            // Warmup
            Console.WriteLine("Warming up...");
            EF6.DataInitializer.Warmup(clearData);
            EFCore.DataInitializer.Warmup(clearData, _options);
        }

        private static void CompareSave()
        {
            Console.WriteLine($"Populating database with {Program.TABLE_SIZE} people");

            Stopwatch watch = Stopwatch.StartNew();
            EF6.DataInitializer.LoadLotsOfData();
            watch.Stop();
            Console.WriteLine($"  - EF6.x:      {watch.ElapsedMilliseconds}ms");

            watch.Restart();
            EFCore.DataInitializer.LoadLotsOfData(_options);
            watch.Stop();
            Console.WriteLine($"  - EF Core:    {watch.ElapsedMilliseconds}ms");
        }

        private static void CompareFetch()
        {
            Console.WriteLine($"Querying databases with {Program.TABLE_SIZE} people");
            CompareSimpleQuery();
        }

        private static void CompareSimpleQuery()
        {
            Stopwatch watch = new Stopwatch();

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($" Iteration {i}");

                // With EF 6
                watch.Restart();
                using (var context = new EF6.StarWarsContext())
                {
                    context.People.ToList();
                }
                watch.Stop();
                var prevResult = watch.ElapsedMilliseconds;
                Console.WriteLine($"  - EF6.x:      {watch.ElapsedMilliseconds}ms");

                // With EF Core
                watch.Restart();
                using (var context = new EFCore.StarWarsContext(_options))
                {
                    context.People.ToList();
                }
                watch.Stop();
                Console.WriteLine($"  - EF Core:    {watch.ElapsedMilliseconds}ms");

                // Bottom line
                var improvement = (prevResult - watch.ElapsedMilliseconds) / (double)prevResult;
                Console.WriteLine($"  - Improvement:{improvement.ToString("P0")}");
                Console.WriteLine();
            }

        }


    }
}
