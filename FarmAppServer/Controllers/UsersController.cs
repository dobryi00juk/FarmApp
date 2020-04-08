using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FarmApp.Domain.Core.Entity;
using FarmAppServer.Helpers;
using FarmAppServer.Models;
using FarmAppServer.Models.Users;
using FarmAppServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FarmAppServer.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        public UsersController(IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthResponseDto>> Authenticate([FromBody]AuthenticateModelDto model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            var users = await _userService.AuthenticateUserAsync(model.Username, model.Password);

            if (users == null)
                return BadRequest(new ResponseBody { Result = "Username or password is incorrect", Header = "Authenticate" });
            
            var user = await users.SingleOrDefaultAsync();

            if (user == null)
                return BadRequest(new ResponseBody { Result = "Username or password is incorrect", Header = "Authenticate" });
            
            if (user.IsDeleted ?? true)
                return BadRequest(new ResponseBody { Result = "Пользователь заблокирован!", Header = "Authenticate" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("RoleId", user.RoleId.ToString()) 
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            // return basic user info and authentication token

            var response = await _mapper.ProjectTo<AuthResponseDto>(users).FirstOrDefaultAsync();
            response.Token = tokenString;

            return Ok(response);
            
        }

        //[AllowAnonymous]
        //[Authorize(Roles = "admin")]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterModelDto model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                await _userService.CreateUserAsync(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModelDto>>> GetAll()
        {
            try
            {
                var users = _userService.GetAllUsers();
                var model = await _mapper.ProjectTo<UserModelDto>(users).ToListAsync();
                return Ok(model);
            }
            catch (Exception e)
            {
                return BadRequest(new {e.Message, e.StackTrace});
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserModelDto>> GetById(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                var model = await _mapper.ProjectTo<UserModelDto>(user).FirstOrDefaultAsync();
                return Ok(model);
            }
            catch (Exception e)
            {
                return BadRequest(new {e.Message, e.StackTrace});
            }
            
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UpdateModelDto model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // update user 
                _userService.UpdateUser(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message, ex.StackTrace });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _userService.DeleteUser(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message, e.StackTrace });
            }

        }
    }
}
