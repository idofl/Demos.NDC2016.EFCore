using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Queries
{
    class Program
    {
        static DbContextOptionsBuilder<EFCore.StarWarsContext> _optionsBuilder;

        static void Main(string[] args)
        {
            _optionsBuilder = new DbContextOptionsBuilder<EFCore.StarWarsContext>();
            _optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;database=EFCore.Queries.Core;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            EFCore.DataInitializer.InitDatabase(_optionsBuilder.Options);
            EF6.DataInitializer.InitDatabase();

            //EagerLoad();
            //MixClientAndSql();
        }

        private static void MixClientAndSql()
        {
            using (var context = new EFCore.StarWarsContext(_optionsBuilder.Options))
            {
                var query = context.People
                    .FromSql($"Select * from people where {nameof(EFCore.Person.Height)} > {{0}}", 1.5)
                    .OrderByDescending(p => p.Height);

                Console.WriteLine("With EF Core:");
                query.ToList().ForEach(
                    p => Console.WriteLine($"{p.Name}-{p.Height}"));
            }
            Console.WriteLine();
            using (var context = new EF6.StarWarsContext())
            {
                var query = context.People
                    .SqlQuery($"Select * from people where {nameof(EFCore.Person.Height)} > {{0}}", 1.5)
                    .OrderByDescending(p => p.Height);

                Console.WriteLine("With EF 6:");
                query.ToList().ForEach(
                    p => Console.WriteLine($"{p.Name}-{p.Height}"));
            }
        }

        private static void EagerLoad()
        {
            using (var context = new EFCore.StarWarsContext(_optionsBuilder.Options))
            {
                var query = from p in context.People.Include(person => person.Starships)
                            where p.Name.StartsWith("Luke")
                            select p;

                Console.WriteLine("With EF Core:");
                query.ToList().ForEach(p =>
                {
                    Console.WriteLine(p.Name);
                    p.Starships.ForEach(s =>
                    {
                        Console.WriteLine($"   {s.Name}");
                    });
                });
            }
            Console.WriteLine();
            using (var context = new EF6.StarWarsContext())
            {
                var query = from p in System.Data.Entity.QueryableExtensions.Include(
                                context.People, person => person.Starships)
                            where p.Name.StartsWith("Luke")
                            select p;

                Console.WriteLine("With EF 6:");
                query.ToList().ForEach(p =>
                {
                    Console.WriteLine(p.Name);
                    p.Starships.ForEach(s =>
                    {
                        Console.WriteLine($"   {s.Name}");
                    });
                });
            }
        }
    }
}
