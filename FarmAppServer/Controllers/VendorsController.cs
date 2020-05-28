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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.OpenApi.Validations;

namespace FarmAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly FarmAppContext _context;
        private readonly IMapper _mapper;

        public VendorsController(FarmAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Vendors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendorDto>>> GetVendors()
        {
            var vendors = _context.Vendors;

            if (vendors == null)
                return NotFound(new ResponseBody()
                {
                    Header = "Error",
                    Result = "Vendors not found"
                });

            var model = await _mapper.ProjectTo<VendorDto>(vendors).ToListAsync();
            
            return model;
        }

        // GET: api/Vendors/5
        [HttpGet("VendorById")]
        public async Task<ActionResult<VendorDto>> GetVendor([FromQuery]int id)
        {
             var vendor = await _context.Vendors.FindAsync(id);

             if (vendor == null)
                 return NotFound();

             if (vendor.IsDeleted == true)
                 return BadRequest(new ResponseBody()
                 {
                     Header = "Error",
                     Result = "Vendor not found"
                 });
             
             var result = _mapper.Map<VendorDto>(vendor);

             return Ok(result);
        }

        // PUT: api/Vendors/5
        [HttpPut]
        public async Task<ActionResult<VendorDto>> PutVendor([FromQuery]int id, [FromBody]JsonPatchDocument model)// Vendor model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var vendor = await _context.Vendors.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (vendor == null)
                return NotFound(new ResponseBody()
                {
                    Header = "Error",
                    Result = "Vendor nor found"
                });

            _mapper.Map(model, vendor);
            _context.Update(vendor);
            await _context.SaveChangesAsync();

            var result = _mapper.Map<VendorDto>(vendor);

            return Ok(result);
        }

        // POST: api/Vendors
        [HttpPost]
        public async Task<ActionResult<VendorDto>> PostVendor([FromBody]VendorDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var vendor = _mapper.Map<Vendor>(model);
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();

            return Created("PostRegion", vendor);
        }

        // DELETE: api/Vendors/5
        [HttpDelete]
        public async Task<ActionResult<VendorDto>> DeleteVendor([FromQuery]int id)
        {
             var vendor = await _context.Vendors.FindAsync(id);

             if (vendor == null)
                 return NotFound(new ResponseBody()
                 {
                     Header = "Error",
                     Result = "vendor not found"
                 });

             //_context.Vendors.Remove(vendor);
             vendor.IsDeleted = true;
             await _context.SaveChangesAsync();

             var result = _mapper.Map<VendorDto>(vendor);

             return result;
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.Id == id && e.IsDeleted == false);
        }
    }
}
