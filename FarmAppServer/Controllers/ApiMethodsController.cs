using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Authorization;

namespace FarmAppServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiMethodsController : ControllerBase
    {
        private readonly FarmAppContext _context;

        public ApiMethodsController(FarmAppContext context)
        {
            _context = context;
        }

        // GET: api/ApiMethods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiMethod>>> GetApiMethods()
        {
            return await _context.ApiMethods.ToListAsync();
        }

        // GET: api/ApiMethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiMethod>> GetApiMethod(int id)
        {
            var apiMethod = await _context.ApiMethods.FindAsync(id);

            if (apiMethod == null)
            {
                return NotFound();
            }

            return apiMethod;
        }

        // PUT: api/ApiMethods/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApiMethod(int id, ApiMethod apiMethod)
        {
            if (id != apiMethod.Id)
            {
                return BadRequest();
            }

            _context.Entry(apiMethod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApiMethodExists(id))
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

        // POST: api/ApiMethods
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ApiMethod>> PostApiMethod(ApiMethod apiMethod)
        {
            _context.ApiMethods.Add(apiMethod);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApiMethod", new { id = apiMethod.Id }, apiMethod);
        }

        // DELETE: api/ApiMethods/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiMethod>> DeleteApiMethod(int id)
        {
            var apiMethod = await _context.ApiMethods.FindAsync(id);
            if (apiMethod == null)
            {
                return NotFound();
            }

            _context.ApiMethods.Remove(apiMethod);
            await _context.SaveChangesAsync();

            return apiMethod;
        }

        private bool ApiMethodExists(int id)
        {
            return _context.ApiMethods.Any(e => e.Id == id);
        }
    }
}
