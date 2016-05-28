using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.Performance.EF6
{
    public class StarWarsContext : DbContext
    {
        public StarWarsContext()
            //:base(@"Server=tcp:idoftest.database.windows.net,1433;Data Source=idoftest.database.windows.net;Initial Catalog=EFCore.Performance.6;Persist Security Info=False;User ID=idof;Password=P@ssw0rd12!;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
            : base(@"Data Source=.\sqlexpress;database=EFCore.Performance.6;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
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
        public List<Starship> Starships { get; set; }
    }

    public class Starship
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int MaxPassengers { get; set; }
        public Person Owner { get; set; }
    }
}
