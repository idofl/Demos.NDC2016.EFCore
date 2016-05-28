using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Queries
{
    class Program
    {
        static void Main(string[] args)
        {
            EFCore.DataInitializer.InitDatabase();
            EF6.DataInitializer.InitDatabase();

            //EagerLoad();
            //MixClientAndSql();
        }

        private static void MixClientAndSql()
        {
            using (var context = new EFCore.StarWarsContext())
            {
                var query = context.People
                    .FromSql($"Select * from person where {nameof(EFCore.Person.Height)} > {{0}}", 1.5)
                    .OrderBy(p => p.Height);

                query.ToList().ForEach(
                    p => Console.WriteLine($"{p.Name}-{p.Height}"));
            }

            using (var context = new EF6.StarWarsContext())
            {
                var query = context.People
                    .SqlQuery($"Select * from people where {nameof(EFCore.Person.Height)} > {{0}}", 1.5)
                    .OrderBy(p => p.Height);

                query.ToList().ForEach(
                    p => Console.WriteLine($"{p.Name}-{p.Height}"));
            }
        }

        private static void EagerLoad()
        {
            using (var context = new EFCore.StarWarsContext())
            {
                var query = from p in context.People.Include(person => person.Starships)
                            where p.Name.StartsWith("Luke")
                            select p;

                query.ToList().ForEach(p => Console.WriteLine(p.Name));
            }

            using (var context = new EF6.StarWarsContext())
            {
                var query = from p in System.Data.Entity.QueryableExtensions.Include(
                                context.People, person => person.Starships)
                            where p.Name.StartsWith("Luke")
                            select p;

                query.ToList().ForEach(p => Console.WriteLine(p.Name));
            }
        }
    }
}
