using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using FarmAppServer.Extantions;
using FarmAppServer.Helpers;
using FarmAppServer.Models;
using FarmAppServer.Models.Users;
using FarmAppServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

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

        #region My auth

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthResponseDto>> Authenticate([FromBody]AuthenticateModelDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            //var model = JsonConvert.DeserializeObject<AuthenticateModelDto>(requestBody.Param);

            if (model == null) throw new ArgumentNullException(nameof(model));

            var users = await _userService.AuthenticateUserAsync(model.Login, model.Password);

            if (users == null)
                return BadRequest(new ResponseBody { Result = "Login or password is incorrect", Header = "Authenticate" });

            var user = await users.SingleOrDefaultAsync();

            if (user == null)
                return BadRequest(new ResponseBody { Result = "Login or password is incorrect", Header = "Authenticate" });

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

        #endregion

        //#region GetToken

        //[HttpPost, Route("GetToken")]
        //public async Task<IActionResult> Auntification([FromBody]RequestBody requestBody)
        //{
        //    var user = JsonConvert.DeserializeObject<User>(requestBody.Param);
        //    user = await _context.Users.FirstOrDefaultAsync(x => x.Login == user.Login && x.Password == user.Password);

        //    if (user == null)
        //        return NotFound(new ResponseBody { Header = "Аунтификация", Result = "Неверный логин или пароль!" });

        //    if (user.IsDeleted ?? true)
        //        return BadRequest(new ResponseBody { Result = "Пользователь заблокирован!", Header = "Аунтификация" });
        //    throw new Exception("сука");

        //    var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId);
        //    if (role == null)
        //        return NotFound(new ResponseBody { Header = "Аунтификация", Result = "Неизвестная роль пользователя!" });

        //    if (role.IsDeleted ?? true)
        //        return BadRequest(new ResponseBody { Header = "Аунтификация", Result = "Роль удалена!" });

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //            new Claim("UserId", user.Id.ToString()),
        //            new Claim("RoleId", role.Id.ToString())
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(1),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        //    var token = tokenHandler.WriteToken(securityToken);
        //    return Ok(new ResponseBody { Header = "Ok", Result = token });
        //}

        //#endregion








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
        public async Task<ActionResult<IEnumerable<UserModelDto>>> GetAll([FromQuery]int page = 1, int pageSize = 25)
        {
            /*
             
            var sales = _context.Regions.Where(x => x.IsDeleted == false);
            
            try
            {
                var query = sales.GetPaged(page, pageSize);

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
            
            */
            
            
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

        [HttpPut("{id}")]///////////&&&?????
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



        //use User service UserFilterByRole
        //⦁	Combobox -> По ролям (Админ, пользователь …)
        [HttpGet]
        [Route("RoleName")]
        public async Task<ActionResult<IEnumerable<UserFilterByRoleDto>>> GetUsersByRoleAsync([FromQuery]string role)
        {
            //&&&???
            if (string.IsNullOrEmpty(role))
                return BadRequest(new
                {
                    Message = $"Value cannot be null or empty.  {nameof(role)}"
                });

            var users = _userService.UsersByRoleAsync(role);
            var model = await _mapper.ProjectTo<UserFilterByRoleDto>(users).ToListAsync();
            return Ok(model);
        }

        //search by Login or UserName
        //⦁	TextBox  -> По Login, UserName
        [HttpGet]
        [Route("Search")]
        public async Task<ActionResult<UserFilterByRoleDto>> SearchUser([FromQuery] string param)
        {
            if (string.IsNullOrEmpty(param))
                return BadRequest(new ResponseBody { Result = "param cannot be null or empty", Header = "Search" });

            var users = _userService.UserSearchAsync(param);
            var model = await _mapper.ProjectTo<UserFilterByRoleDto>(users).ToListAsync();

            return Ok(model);
        }

        //checkbox switch
        [HttpGet]
        [Route("CheckBox")]
        public async Task<ActionResult<IEnumerable<UserFilterByRoleDto>>> CheckBoxFilter([FromQuery] bool isChecked)
        {
            var users = _userService.IsEnabled(isChecked);
            var model = await _mapper.ProjectTo<UserFilterByRoleDto>(users).ToListAsync();

            return model;
        }
    }
}
