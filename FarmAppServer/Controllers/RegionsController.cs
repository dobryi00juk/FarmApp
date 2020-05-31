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
using FarmAppServer.Models.Regions;
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
        public ActionResult<IEnumerable<RegionDto>> GetRegions([FromQuery]int page = 1, [FromQuery]int pageSize = 25)
        {
            var regions = _context.Regions.Include(x => x.Regions);
            
            var model = _mapper.ProjectTo<RegionDto>(regions);
            var query = model.GetPaged(page, pageSize);

            return Ok(query);
        }

        // GET: api/Regions/5
        [HttpGet("RegionById")]
        public async Task<ActionResult<RegionDto>> GetRegion([FromQuery]int id)
        {
            var region = await _context.Regions.FindAsync(id);

            if (region == null || region.IsDeleted == true)
                return NotFound();

            var result = _mapper.Map<RegionDto>(region);
            return result;
        }

        // PUT: api/Regions/5
        [HttpPut]
        public async Task<ActionResult<RegionDto>> PutRegion([FromQuery]int id, [FromBody]UpdateRegionDto model)
        {
            if(!ModelState.IsValid) 
                return BadRequest();

            var updated = await _regionService.UpdateRegionAsync(id, model);

            if (updated)
                return Ok();

            return NotFound(new ResponseBody()
            {
                Header = "Error",
                Result = "user not found"
            });
        }

        [HttpPost]
        public async Task<ActionResult<RegionDto>> PostRegion([FromBody]PostRegionDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var region = _mapper.Map<Region>(model);

            if (region == null) return BadRequest();

            var request = await _regionService.PostRegion(region);
            var result = _mapper.Map<RegionDto>(request);

            return Created("PostRegion", result);
        }

        // DELETE: api/Regions/5
        [HttpDelete]
        public async Task<ActionResult<RegionDto>> DeleteRegion([FromQuery]int id)
        {
            if (await _regionService.DeleteRegionAsync(id))
                return Ok();

            return NotFound(new ResponseBody()
            {
                Header = "Error",
                Result = "User not found"
            });
        }

        private bool RegionExists(int id)
        {
            return _context.Regions.Any(e => e.Id == id && e.IsDeleted == false);
        }

        //Фильтры: TextBox -> RegionName
        [HttpGet("SearchRegion")]
        public async Task<ActionResult<RegionDto>> SearchRegion([FromQuery]string regionName)
        {
            var regions = _regionService.RegionNameSearch(regionName);
            var model = await _mapper.ProjectTo<RegionDto>(regions).ToListAsync();

            //???
            foreach (var item in model.Where(item => item.IsDeleted))
                model.Remove(item);

            return Ok(model);
        }
    }
}
