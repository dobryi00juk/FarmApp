using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;

namespace FarmAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionTypesController : ControllerBase
    {
        private readonly FarmAppContext _context;

        public RegionTypesController(FarmAppContext context)
        {
            _context = context;
        }

        // GET: api/RegionTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionType>>> GetRegionTypes()
        {
            return await _context.RegionTypes.ToListAsync();
        }

        // GET: api/RegionTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegionType>> GetRegionType(int id)
        {
            var regionType = await _context.RegionTypes.FindAsync(id);

            if (regionType == null)
            {
                return NotFound();
            }

            return regionType;
        }

        // PUT: api/RegionTypes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegionType(int id, RegionType regionType)
        {
            if (id != regionType.Id)
            {
                return BadRequest();
            }

            _context.Entry(regionType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegionTypeExists(id))
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

        // POST: api/RegionTypes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<RegionType>> PostRegionType(RegionType regionType)
        {
            _context.RegionTypes.Add(regionType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegionType", new { id = regionType.Id }, regionType);
        }

        // DELETE: api/RegionTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RegionType>> DeleteRegionType(int id)
        {
            var regionType = await _context.RegionTypes.FindAsync(id);
            if (regionType == null)
            {
                return NotFound();
            }

            _context.RegionTypes.Remove(regionType);
            await _context.SaveChangesAsync();

            return regionType;
        }

        private bool RegionTypeExists(int id)
        {
            return _context.RegionTypes.Any(e => e.Id == id);
        }
    }
}
