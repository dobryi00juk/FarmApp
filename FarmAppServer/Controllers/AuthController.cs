using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using FarmAppServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FarmAppServer.Services;

namespace FarmAppServer.Controllers
{
    [Authorize]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ApplicationSettings AppSettings;
        private readonly FarmAppContext Ctx;
        private readonly IUserService _userService;

        public AuthController(FarmAppContext ctx, IOptions<ApplicationSettings> appSettings, IUserService userService)
        {
            Ctx = ctx;
            _userService = userService;
            AppSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost, Route("Authentication")]
        public IActionResult Authentication([FromBody]AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Login, model.Password, out var token);

            if (user == null)
                return NotFound(new ResponseBody { Header = "Аунтификация", Result = "Неверный логин или пароль!"});

            if (user.IsDeleted ?? true)
                return BadRequest(new ResponseBody { Result = "Пользователь заблокирован!", Header = "Аунтификация" });

            var securityToken = token;

            //var role = await Ctx.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId);

            //if (role == null)
            //    return NotFound(new ResponseBody { Header = "Аунтификация", Result = "Неизвестная роль пользователя!" });

            //if (role.IsDeleted ?? true)
            //    return BadRequest(new ResponseBody { Header = "Аунтификация", Result = "Роль удалена!" });

            return Ok(new ResponseBody { Header = "Ok", Result = securityToken });
        }

        //[Access]
        [HttpGet, Route("getuser")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = "1";
            //string roleId = User.Claims.First(c => c.Type == "RoleId").Value;
            //HttpContext.Items.
            var user = await Ctx.Users.FindAsync(int.Parse(userId));
            user.Role = await Ctx.Roles.FindAsync(user.RoleId);
            return Ok(new
            {
                user.UserName,
                user.Login,
                user.Role?.RoleName
            });
        }

        [HttpGet, Route("getusers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await Ctx.Users.ToListAsync();
        }
    }
}
