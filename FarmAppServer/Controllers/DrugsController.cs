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
using FarmAppServer.Models.Drug;
using FarmAppServer.Services;
using FarmAppServer.Services.Paging;

namespace FarmAppServer.Controllers
{
    [Route("api/Drugs")]
    [ApiController]
    public class DrugsController : ControllerBase
    {
        private readonly FarmAppContext _context;
        private readonly IMapper _mapper;
        private readonly IDrugService _drugService;

        public DrugsController(FarmAppContext context, IMapper mapper, IDrugService drugService)
        {
            _context = context;
            _mapper = mapper;
            _drugService = drugService;
        }

        // GET: api/Drugs
        [HttpGet]
        public ActionResult<IEnumerable<Drug>> GetDrugs([FromQuery]int page = 1 ,[FromQuery]int pageSize = 25)
        {
            var drugs = _context.Drugs.Where(x => x.IsDeleted == false);
            
            try
            {
                var query = drugs.GetPaged(page, pageSize);

                return Ok(query);
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseBody()
                {
                    Header = "Error",
                    Result = $"{e.Message}"
                });
            }
        }

        // GET: api/Drugs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Drug>> GetDrug(int id)
        {
            var drug = await _context.Drugs.FindAsync(id);

            if (drug == null)
            {
                return NotFound();
            }

            if (drug.IsDeleted == true)
            {
                return NotFound();
            }

            return drug;
        }

        // PUT: api/Drugs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDrug(int id, Drug drug)
        {
            if (id != drug.Id)
            {
                return BadRequest();
            }

            _context.Entry(drug).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrugExists(id))
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

        // POST: api/Drugs
        [HttpPost]
        public async Task<ActionResult<DrugDto>> PostDrug([FromBody]DrugDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            try
            {
                var drug = _mapper.Map<Drug>(model);
                var result = await _drugService.PostDrug(drug);

                return Created("PostDrug", result);
            }
            catch (Exception e)
            {
                return BadRequest(new ResponseBody
                {
                    Header = "Error",
                    Result = $"{e.Message}"
                });
            }
            
        }

        // DELETE: api/Drugs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Drug>> DeleteDrug(int id)
        {
            try
            {
                var drug = await _context.Drugs.FindAsync(id);
                if (drug == null)
                {
                    return NotFound();
                }

                //_context.Drugs.Remove(drug);
                drug.IsDeleted = true;
                await _context.SaveChangesAsync();

                return drug;
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message, e.StackTrace });
            }
        }

        private bool DrugExists(int id)
        {
            return _context.Drugs.Any(e => e.Id == id && e.IsDeleted == false);
        }
    }
}
