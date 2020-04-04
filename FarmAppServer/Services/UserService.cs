using System;
using System.Collections.Generic;
using System.Linq;
using FarmApp.Domain.Core.Entity;
using FarmAppServer.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings;
using FarmApp.Infrastructure.Data.Contexts;

namespace FarmAppServer.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(int id);
    }
    
    public class UserService : IUserService
    {
        private readonly FarmAppContext _context;

        public UserService(FarmAppContext context)
        {
            _context = context;
        }
        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.UserName == username);

            if (user==null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.UserName == user.UserName))
                throw new AppException("Username \"" + user.UserName + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            
            _context.Users.Add(user);
            _context.SaveChangesAsync();

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.Id);

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
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChangesAsync();
            }
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