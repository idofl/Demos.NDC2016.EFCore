using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.Extensions.Caching.Memory;
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
                .AddEntityFrameworkSqlServer();

            // Replace the batch executor with a custom wrapper implementation
            serviceCollection.AddTransient<IBatchExecutor, CustomBatchExecutor>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var optionsBuilder = new DbContextOptionsBuilder<StarWarsContext>();
            optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;database=EFCore.Services;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            optionsBuilder.UseInternalServiceProvider(serviceProvider);

            using (StarWarsContext context = new StarWarsContext(optionsBuilder.Options))
            {
                serviceProvider = context.GetInfrastructure<IServiceProvider>();
                
                // Add a custom console logger, and a debug logger
                // (can also be added to the serviceProvider before calling ctor)
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                loggerFactory.AddProvider(new ConsoleLoggerProvider(LogLevel.Information));
                loggerFactory.AddDebug(LogLevel.Debug);

                context.Database.EnsureCreated();

                for (int i = 1; i < 10; i++)
                {
                    Person person = new Person
                    {
                        Name = $"Luke Skywalker #{i}",
                        HairColor = "Blond",
                        Height = 1.72                  
                    };

                    context.People.Add(person);
                }
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
        public StarWarsContext(DbContextOptions<StarWarsContext> options)
            : base(options)
        {

        }
      
        public DbSet<Person> People { get; set; }    
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HairColor { get; set; }
        public double Height { get; set; }
    }
}
