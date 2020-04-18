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
            try
            {
                var vendors = _context.Vendors.Where(x => x.IsDeleted == false);
                var model = await _mapper.ProjectTo<VendorDto>(vendors).ToListAsync();
                return model;
            }
            catch (Exception e)
            {
                return BadRequest(new {e.Message, e.StackTrace});
            }
        }

        // GET: api/Vendors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            try
            {
                var vendor = await _context.Vendors.FindAsync(id);

                if (vendor == null)
                {
                    return NotFound();
                }

                if (vendor.IsDeleted == true)
                {
                    return BadRequest(new ResponseBody()
                    {
                        Header = "Error",
                        Result = "Роль не найдена"
                    });
                }

                return vendor;
            }
            catch (Exception e)
            {
                return BadRequest(new {e.Message, e.StackTrace});
            }
        }

        // PUT: api/Vendors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVendor(int id, Vendor vendor)
        {
            if (id != vendor.Id)
            {
                return BadRequest();
            }

            _context.Entry(vendor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists(id))
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

        // POST: api/Vendors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Vendor>> PostVendor(VendorDto vendorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var vendor = _mapper.Map<Vendor>(vendorDto);
                _context.Vendors.Add(vendor);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetVendor", new { id = vendor.Id }, vendor);
            }
            catch (Exception e)
            {
                return BadRequest(new {e.Message, e.StackTrace});
            }
        }

        // DELETE: api/Vendors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Vendor>> DeleteVendor(int id)
        {
            try
            {
                var vendor = await _context.Vendors.FindAsync(id);
                if (vendor == null)
                {
                    return NotFound();
                }

                //_context.Vendors.Remove(vendor);
                vendor.IsDeleted = true;
                await _context.SaveChangesAsync();

                return vendor;
            }
            catch (Exception e)
            {
                BadRequest(new {e.Message, e.StackTrace});
                throw;
            }
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.Id == id && e.IsDeleted == false);
        }
    }
}
