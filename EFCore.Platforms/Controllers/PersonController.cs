using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EFCore.Platforms.Models;

namespace EFCore.Platforms.Controllers
{
    [Route("api/[controller]")]
    public class PersonController : Controller
    {
        StarWarsContext _context;
        public PersonController(StarWarsContext context)
        {
            _context = context;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return _context.People.ToList();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }    
    }
}
