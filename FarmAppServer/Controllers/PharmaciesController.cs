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
using FarmAppServer.Helpers;
using FarmAppServer.Models;
using FarmAppServer.Models.Pharmacy;
using FarmAppServer.Services;
using FarmAppServer.Services.Paging;

namespace FarmAppServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PharmaciesController : ControllerBase
    {
        private readonly FarmAppContext _context;
        private readonly IMapper _mapper;
        private readonly IPharmacyService _pharmacyService;

        public PharmaciesController(FarmAppContext context, IMapper mapper, IPharmacyService pharmacyService)
        {
            _context = context;
            _mapper = mapper;
            _pharmacyService = pharmacyService;
        }

        // GET: api/Pharmacies
        [HttpGet]
        public ActionResult<IEnumerable<PharmacyFilterDto>> GetPharmacies([FromQuery]int page = 1, int pageSize = 25)
        {
            var pharmacies = _context.Pharmacies.Where(x => x.IsDeleted == false);
            
            try
            {
                var model = _mapper.ProjectTo<PharmacyFilterDto>(pharmacies);
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
        }

        // GET: api/Pharmacies/5
        [HttpGet("GetById")]
        public async Task<ActionResult<Pharmacy>> GetPharmacy([FromQuery]int id)
        {
            var pharmacy = await _context.Pharmacies.FindAsync(id);

            if (pharmacy == null)
            {
                return NotFound(new ResponseBody()
                {
                    Header = "Error",
                    Result = "Pharmacy not found"
                });
            }

            if (pharmacy.IsDeleted == true)
            {
                return BadRequest(new ResponseBody()
                {
                    Header = "Error",
                    Result = "Pharmacy not found"
                });
            }

            return pharmacy;
        }

        // PUT: api/Pharmacies/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut]
        public IActionResult PutPharmacy([FromQuery]int id, [FromBody]PharmacyDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var pharmacy = _mapper.Map<Pharmacy>(model);
            pharmacy.Id = id;

            try
            {
                _pharmacyService.UpdatePharmacyAsync(pharmacy);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new {message = ex.Message, ex.StackTrace});
            }
        }

        // POST: api/Pharmacies
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Pharmacy>> PostPharmacy([FromBody]PharmacyDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
                
            try
            {
                var pharmacy = _mapper.Map<Pharmacy>(model);
                var result = await _pharmacyService.PostPharmacyAsync(pharmacy);

                return Created("PostPharmacy", result);
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

        // DELETE: api/Pharmacies/5
        [HttpDelete]
        public async Task<ActionResult<Pharmacy>> DeletePharmacy([FromQuery]int id)
        {
            var pharmacy = await _context.Pharmacies.FindAsync(id);
            if (pharmacy == null)
            {
                return NotFound();
            }

            //_context.Pharmacies.Remove(pharmacy);
            pharmacy.IsDeleted = true;
            await _context.SaveChangesAsync();

            return pharmacy;
        }

        private bool PharmacyExists(int id)
        {
            return _context.Pharmacies.Any(e => e.Id == id && e.IsDeleted == false);
        }


        //Фильтры: TextBox -> PharmacyName
        [HttpGet("Search")]
        public async Task<ActionResult<PharmacyFilterDto>> SearchAsync([FromQuery] string pharmacyName)
        {
            try
            {
                var pharmacies = _pharmacyService.SearchPharmacy(pharmacyName);
                var model = await _mapper.ProjectTo<PharmacyFilterDto>(pharmacies).ToListAsync();

                return Ok(model);
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
    }
}
