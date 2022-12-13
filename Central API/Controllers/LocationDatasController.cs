using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Central_API.Data;
using Central_API.Models;

namespace Central_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationDatasController : ControllerBase
    {
        private readonly Central_APIContext _context;

        public LocationDatasController(Central_APIContext context)
        {
            _context = context;
        }

        // GET: api/LocationDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationData>>> GetLocationData()
        {
            return await _context.LocationData/*.Include(l => l.Team)*/.ToListAsync();
        }

        // GET: /MostRecent
        [HttpGet("/mostRecent")]
        public async Task<ActionResult<LocationData>> GetMostRecent()
        {
            var list = await _context.LocationData.ToListAsync();
            return list.OrderByDescending(x => x.CreatedAt).FirstOrDefault(); 
        }


        // GET: api/LocationDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationData>> GetLocationData(int id)
        {
            var locationData = await _context.LocationData.FindAsync(id);

            if (locationData == null)
            {
                return NotFound();
            }

            return locationData;
        }

        // PUT: api/LocationDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocationData(int id, LocationData locationData)
        {
            if (id != locationData.Id)
            {
                return BadRequest();
            }

            _context.Entry(locationData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/LocationDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost()]
        public async Task<ActionResult<LocationData>> PostLocationData([FromBody] LocationData locationData)
        {
            //TODO calculate distance
            locationData.CreatedAt = DateTime.Now;  
            _context.Add(locationData);
            await _context.SaveChangesAsync();

            return locationData;
        }

        // DELETE: api/LocationDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocationData(int id)
        {
            var locationData = await _context.LocationData.FindAsync(id);
            if (locationData == null)
            {
                return NotFound();
            }

            _context.LocationData.Remove(locationData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationDataExists(int id)
        {
            return _context.LocationData.Any(e => e.Id == id);
        }
    }
}
