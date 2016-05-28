using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Services
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddLogging()
                .AddEntityFramework()
                .AddSqlServer();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            //loggerFactory.AddConsole(LogLevel.Debug);

            using (StarWarsContext context = new StarWarsContext(serviceProvider))
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
        public StarWarsContext(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {

        }
        public DbSet<Person> People { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;database=EFCore.Services;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
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
