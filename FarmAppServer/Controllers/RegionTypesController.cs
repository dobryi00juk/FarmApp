﻿using System;
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

namespace FarmAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionTypesController : ControllerBase
    {
        private readonly FarmAppContext _context;
        private readonly IRegionTypeService _regionTypeService;
        private readonly IMapper _mapper;

        public RegionTypesController(FarmAppContext context, IRegionTypeService regionTypeService, IMapper mapper)
        {
            _context = context;
            _regionTypeService = regionTypeService;
            _mapper = mapper;
        }

        // GET: api/RegionTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegionType>>> GetRegionTypes()
        {
            return await _context.RegionTypes
                .Include(x => x.Regions)
                .ToListAsync();//.Where(x => x.IsDeleted == false).ToListAsync();
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

            if (regionType.IsDeleted == true)
            {
                return BadRequest(new ResponseBody()
                {
                    Header = "Error",
                    Result = "Роль не найдена"
                });
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

            //_context.RegionTypes.Remove(regionType);
            regionType.IsDeleted = true;
            await _context.SaveChangesAsync();

            return regionType;
        }

        private bool RegionTypeExists(int id)
        {
            return _context.RegionTypes.Any(e => e.Id == id && e.IsDeleted == false);
        }
        

        //Фильтр: Текстбокс по RegionTypes и IsEnabled
        [HttpGet("RegionTypeSearch")]
        public async Task<ActionResult<RegionTypeDto>> RegionTypeSearch([FromQuery]string param, bool isEnabled)
        {
            if (string.IsNullOrWhiteSpace(param))
                return BadRequest(new ResponseBody()
                {
                    Header = "Error",
                    Result = $"Value cannot be null or whitespace. {nameof(param)}"
                });
            
            var regionsType = _regionTypeService.SearchRegionType(param);
            var model = await _mapper.ProjectTo<RegionTypeDto>(regionsType).ToListAsync();

            return Ok(model.FirstOrDefault());
        }
    }
}
