using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using FarmAppServer.Models;

namespace FarmAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeAthTypesController : ControllerBase
    {
        private readonly FarmAppContext _context;

        public CodeAthTypesController(FarmAppContext context)
        {
            _context = context;
        }

        // GET: api/CodeAthTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CodeAthType>>> GetCodeAthTypes()
        {
            return await _context.CodeAthTypes.Where(x => x.IsDeleted == false).ToListAsync();
        }

        // GET: api/CodeAthTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CodeAthType>> GetCodeAthType(int id)
        {
            var codeAthType = await _context.CodeAthTypes.FindAsync(id);

            if (codeAthType == null)
            {
                return NotFound();
            }

            if (codeAthType.IsDeleted == true)
            {
                return BadRequest(new ResponseBody()
                {
                    Header = "Error",
                    Result = "Роль не найдена"
                });
            }

            return codeAthType;
        }

        // PUT: api/CodeAthTypes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCodeAthType(int id, CodeAthType codeAthType)
        {
            if (id != codeAthType.Id)
            {
                return BadRequest();
            }

            _context.Entry(codeAthType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CodeAthTypeExists(id))
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

        // POST: api/CodeAthTypes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CodeAthType>> PostCodeAthType(CodeAthType codeAthType)
        {
            _context.CodeAthTypes.Add(codeAthType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCodeAthType", new { id = codeAthType.Id }, codeAthType);
        }

        // DELETE: api/CodeAthTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CodeAthType>> DeleteCodeAthType(int id)
        {
            var codeAthType = await _context.CodeAthTypes.FindAsync(id);
            if (codeAthType == null)
            {
                return NotFound();
            }

            //_context.CodeAthTypes.Remove(codeAthType);
            codeAthType.IsDeleted = true;
            await _context.SaveChangesAsync();

            return codeAthType;
        }

        private bool CodeAthTypeExists(int id)
        {
            return _context.CodeAthTypes.Any(e => e.Id == id && e.IsDeleted == false);
        }
    }
}
