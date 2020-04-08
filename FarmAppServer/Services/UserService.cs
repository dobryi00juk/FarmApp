using System;
using System.Collections.Generic;
using System.Linq;
using FarmApp.Domain.Core.Entity;
using FarmAppServer.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings;
using System.Threading.Tasks;
using FarmApp.Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace FarmAppServer.Services
{
    public interface IUserService
    {
        Task<IQueryable<User>> AuthenticateUserAsync(string username, string password);
        IQueryable GetAllUsers();
        IQueryable GetUserById(int id);
        Task<User> CreateUserAsync(User user, string password);
        void UpdateUser(User user, string password = null);
        void DeleteUser(int id);
    }
    
    public class UserService : IUserService
    {
        private readonly FarmAppContext _context;

        public UserService(FarmAppContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<User>> AuthenticateUserAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var users = _context.Users.Where(x => x.UserName == username);
            var user = await users.FirstOrDefaultAsync();
            
            if (user==null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return users;
        }

        public IQueryable GetAllUsers()
        {
            return _context.Users;
        }

        public IQueryable GetUserById(int id)
        {
            //return await _context.Users.FindAsync(id);
            return _context.Users.Where(x => x.Id == id);
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.UserName == user.UserName))
                throw new AppException("Username \"" + user.UserName + "\" is already taken");

            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public void UpdateUser(User userParam, string password = null)
        {
            //var user = _context.Users.Find(userParam.Id);
            var user = _context.Users.FirstOrDefault(x => x.Id == userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (!string.IsNullOrWhiteSpace(userParam.UserName) && userParam.UserName != user.UserName)
            {
                if (_context.Users.Any(x => x.UserName == userParam.UserName))
                    throw new AppException("UserNAme " + userParam.UserName + " is already taken");

                user.UserName = userParam.UserName;
            }
            
            //update
            if (!string.IsNullOrWhiteSpace(userParam.FirstName)) user.FirstName = userParam.FirstName;
            if (!string.IsNullOrWhiteSpace(userParam.LastName)) user.LastName = userParam.LastName;
            
            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);

            if (user == null) return ;
            if (user.IsDeleted == true) return;
            //_context.Users.Remove(user);
            user.IsDeleted = true;
            _context.SaveChanges();
        }
        
        //private helper method

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));

            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
            
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}