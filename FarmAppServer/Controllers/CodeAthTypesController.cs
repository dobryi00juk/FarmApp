using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using FarmAppServer.Models;
using FarmAppServer.Models.CodeAthTypes;
using FarmAppServer.Models.Sales;
using FarmAppServer.Services;
using FarmAppServer.Services.Paging;

namespace FarmAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeAthTypesController : ControllerBase
    {
        private readonly FarmAppContext _context;
        private readonly IMapper _mapper;
        private readonly ICodeAthService _codeAthService;

        public CodeAthTypesController(FarmAppContext context, IMapper mapper, ICodeAthService codeAthService)
        {
            _context = context;
            _mapper = mapper;
            _codeAthService = codeAthService;
        }

        // GET: api/CodeAthTypes
        [HttpGet]
        public ActionResult<IEnumerable<CodeAthTypeDto>> GetCodeAthTypes([FromQuery]int page = 1, [FromQuery]int pageSize = 25)
        {
            var codesAthType = _codeAthService.GetCodeAthTypes();
            var result = codesAthType.GetPaged(page, pageSize);

            if (result == null)
                return NotFound("CodeAthType not found");

            return Ok(result);
        }

        // GET: api/CodeAthTypes/5
        [HttpGet("CodeAthById")]
        public async Task<ActionResult<CodeAthType>> GetCodeAthType([FromQuery]int id)
        {
            var codeAthType = await _codeAthService.GetCodeAthTypeById(id);

            if (codeAthType == null)
                return NotFound("CodeAthType not found");

            return Ok(codeAthType);
        }

        // PUT: api/CodeAthTypes/5
        [HttpPut]
        public async Task<IActionResult> PutCodeAthType([FromQuery]int id, [FromBody]CodeAthType codeAthType)
        {
            if (id != codeAthType.Id)
                return BadRequest();

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
        [HttpPost]
        public async Task<ActionResult<CodeAthType>> PostCodeAthType([FromBody]PostCodeAthType model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var codeAthType = _mapper.Map<CodeAthType>(model);
            var request = await _codeAthService.PostCodeAthTypeAsync(codeAthType);
            var result = _mapper.Map<CodeAthTypeDto>(request);

            return Created("PostCodeAthType", result);
        }

        // DELETE: api/CodeAthTypes/5
        [HttpDelete]
        public async Task<ActionResult<CodeAthType>> DeleteCodeAthType([FromQuery]int id)
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
