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
using FarmAppServer.Services;
using FarmAppServer.Services.Paging;

namespace FarmAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly FarmAppContext _context;
        private readonly IRegionService _regionService;
        private readonly IMapper _mapper;

        public RegionsController(FarmAppContext context, IRegionService regionService, IMapper mapper)
        {
            _context = context;
            _regionService = regionService;
            _mapper = mapper;
        }

        // GET: api/Regions
        [HttpGet]
        public ActionResult<IEnumerable<RegionDto>> GetRegions([FromQuery]int page = 1, int pageSize = 25)
        {
            var regions = _context.Regions.Where(x => x.IsDeleted == false);
            
            try
            {
                var model = _mapper.ProjectTo<RegionDto>(regions);
                var query = model.GetPaged(page, pageSize);

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
            
            // return await _context.Regions.Where(x => x.IsDeleted == false)
            // .Include(x => x.RegionType)
            // .ToListAsync();
        }

        // GET: api/Regions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Region>> GetRegion(int id)
        {
            var region = await _context.Regions.FindAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            if (region.IsDeleted == true)
            {
                return BadRequest(new ResponseBody()
                {
                    Header = "Error",
                    Result = "Region not found!"
                });
            }

            return region;
        }

        // PUT: api/Regions/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegion(int id, Region region)
        {
            if (id != region.Id)
            {
                return BadRequest();
            }

            _context.Entry(region).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegionExists(id))
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

        // POST: api/Regions
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Region>> PostRegion(Region region)
        {
            _context.Regions.Add(region);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegion", new { id = region.Id }, region);
        }

        // DELETE: api/Regions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Region>> DeleteRegion(int id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            //_context.Regions.Remove(region);
            region.IsDeleted = true;
            await _context.SaveChangesAsync();

            return region;
        }

        private bool RegionExists(int id)
        {
            return _context.Regions.Any(e => e.Id == id && e.IsDeleted == false);
        }

        //Фильтры: TextBox -> RegionName
        [HttpGet("SearchRegion")]
        public async Task<ActionResult<RegionDto>> SearchRegion(string regionName)
        {
            if (string.IsNullOrEmpty(regionName))
                return BadRequest(new ResponseBody()
                {
                    Header = "Error",
                    Result = $"Value cannot be null or empty. {nameof(regionName)}"
                });

            var regions = _regionService.RegionNameSearch(regionName);
            var model = await _mapper.ProjectTo<RegionDto>(regions).ToListAsync();

            return Ok(model);
        }

        //Фильтры: CheckBoxCombobox(Мультивыбор) -> ParentRegionName
        //public async Task<ActionResult<>>

    }
}
