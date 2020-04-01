using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using FarmAppServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FarmAppServer.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password, out string token);
        IEnumerable<User> GetAll();
    }

    public class UserService : IUserService
    {
        private readonly FarmAppContext _context;
        private readonly ApplicationSettings _appSettings;

        public UserService(FarmAppContext context, IOptions<ApplicationSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }
        public User Authenticate(string login, string password, out string token)
        {
            var user = _context.Users.SingleOrDefault(x => x.Login == login && x.Password == password);

            if (Equals(user, null))
            {
                token = null;
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWT_Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            token = tokenHandler.WriteToken(securityToken);
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }
    }
}
