using System;
using System.Collections.Generic;
using System.Linq;
using FarmApp.Infrastructure.Data.Contexts;

namespace FarmAppServer.Services
{
    public interface IRoleService
    {
        public IQueryable RoleSearch(string param);
    }
    public class RoleService : IRoleService
    {
        private readonly FarmAppContext _context;

        public RoleService(FarmAppContext context)
        {
            _context = context;
        }


        //⦁	TextBox по всем полям
        public IQueryable RoleSearch(string param)
        {
            if (string.IsNullOrEmpty(param))
                throw new ArgumentException("Value cannot be null or empty.", nameof(param));

            var query = _context.Roles.Where(x => x.RoleName.Contains(param));

            return query;
        }
    }
}