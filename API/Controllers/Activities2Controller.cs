using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Activities2Controller : ControllerBase
    {
        private readonly DataContext _context;

        public Activities2Controller(DataContext context)
        {
           _context = context;
        }

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> Get()
        {
            return await _context.Activities.ToListAsync();
         }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> Get(int id)
        {
            //return  await _context.Values.FirstOrDefaultAsync(id);
             return  await _context.Activities.FindAsync(id);
            
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var obj =  _context.Activities.FindAsync(id);
            if (obj != null)
            {
                _context.Remove(obj);
                _context.SaveChanges ();
            }
        }
    }
}
