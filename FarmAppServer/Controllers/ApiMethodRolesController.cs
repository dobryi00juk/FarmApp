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
    public class ApiMethodRolesController : ControllerBase
    {
        private readonly FarmAppContext _context;

        public ApiMethodRolesController(FarmAppContext context)
        {
            _context = context;
        }

        // GET: api/ApiMethodRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiMethodRole>>> GetApiMethodRoles()
        {
            return await _context.ApiMethodRoles.ToListAsync();
        }

        // GET: api/ApiMethodRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiMethodRole>> GetApiMethodRole(int id)
        {
            var apiMethodRole = await _context.ApiMethodRoles.FindAsync(id);

            if (apiMethodRole == null)
            {
                return NotFound();
            }

            return apiMethodRole;
        }

        // PUT: api/ApiMethodRoles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApiMethodRole(int id, ApiMethodRole apiMethodRole)
        {
            if (id != apiMethodRole.Id)
            {
                return BadRequest();
            }

            _context.Entry(apiMethodRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApiMethodRoleExists(id))
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

        // POST: api/ApiMethodRoles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ApiMethodRole>> PostApiMethodRole(ApiMethodRole apiMethodRole)
        {
            _context.ApiMethodRoles.Add(apiMethodRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApiMethodRole", new { id = apiMethodRole.Id }, apiMethodRole);
        }

        // DELETE: api/ApiMethodRoles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiMethodRole>> DeleteApiMethodRole(int id)
        {
            var apiMethodRole = await _context.ApiMethodRoles.FindAsync(id);
            if (apiMethodRole == null)
            {
                return NotFound();
            }

            _context.ApiMethodRoles.Remove(apiMethodRole);
            await _context.SaveChangesAsync();

            return apiMethodRole;
        }

        private bool ApiMethodRoleExists(int id)
        {
            return _context.ApiMethodRoles.Any(e => e.Id == id);
        }
    }
}
