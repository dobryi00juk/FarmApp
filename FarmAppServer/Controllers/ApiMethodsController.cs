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
using FarmAppServer.Services.Paging;
using Microsoft.AspNetCore.Authorization;

namespace FarmAppServer.Controllers
{
    //[Authorize]
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
        public ActionResult<IEnumerable<ApiMethod>> GetApiMethods([FromQuery]int page = 1, int pageSize = 20)
        {
            var apiMethods = _context.ApiMethods.Where(x => x.IsDeleted == false);
            
            try
            {
                var query = apiMethods.GetPaged(page, pageSize);

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

        // GET: api/ApiMethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiMethod>> GetApiMethod(int id)
        {
            var apiMethod = await _context.ApiMethods.FindAsync(id);

            if (apiMethod == null)
            {
                return NotFound(new ResponseBody()
                {
                    Header = "Error",
                    Result = "Api method not found!"
                });
            }

            if (apiMethod.IsDeleted == true)
            {
                return BadRequest(new ResponseBody()
                {
                    Header = "Error",
                    Result = "Role not found"
                });
            }

            return apiMethod;
        }

        // PUT: api/ApiMethods/5
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

            //_context.ApiMethods.Remove(apiMethod);
            apiMethod.IsDeleted = true;
            await _context.SaveChangesAsync();

            return apiMethod;
        }

        private bool ApiMethodExists(int id)
        {
            return _context.ApiMethods.Any(e => e.Id == id && e.IsDeleted == false);
        }
    }
}
