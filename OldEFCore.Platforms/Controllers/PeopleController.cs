using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EFCore.Platforms.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Platforms.Controllers
{
    [Route("api/[controller]")]
    public class PeopleController : Controller
    {
        StarWarsContext _context;
        public PeopleController(StarWarsContext context)
        {
            _context = context;
        }

        // GET api/people
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return _context.People.ToList();
        }

        // GET api/people/5
        [HttpGet("{id}", Name ="GetPerson")]
        public Person Get(int id)
        {
            var query = from p in _context.People.Include(p => p.Starships)
                        where p.Id == id
                        select p;

            return query.FirstOrDefault();
        }

        /*
         {
            "Name":"Chewbacca",
            "HairColor":"Brown",
            "Height":2.28,
            "SwapiUrl": "http://swapi.co/api/people/13/",
            "Starships":[
                {"Id":2,"Name":"Millennium Falcon","Cost":100000,"MaxPassengers":6}
            ]
        }

         */
        [HttpPost]
        public IActionResult Post([FromBody]Person person)
        {
            // Calling .Add will mark all entities in the graph as Added
            //_context.People.Add(person);
            
            // Calling .Attach will mark all entities in the graph as unmodified
            //_context.People.Attach(person);
            
            // Setting the state of a single entity will not attach related entities
            //_context.Entry(person).State = EntityState.Added;

            // Traverse the graph and apply custom rules to decide when
            // an entity is added, and when it is modified            
            
            _context.ChangeTracker.TrackGraph(person,
                node =>
                {                    
                    if (node.Entry.Entity is Starship)
                    {
                        if ((node.Entry.Entity as Starship).Id > 0)
                        {
                            node.Entry.State = EntityState.Modified; 
                        }
                        else
                        {
                            node.Entry.State = EntityState.Added;
                        }
                    } 
                    else
                    {
                        node.Entry.State = EntityState.Added;
                    }
                });
                
            try
            {
                _context.Entry(person).Property("LastUpdated").CurrentValue = DateTime.Now;
                _context.SaveChanges();
                return new CreatedAtRouteResult("GetPerson", new { id = person.Id }, person);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }
            
        }
    }
}
