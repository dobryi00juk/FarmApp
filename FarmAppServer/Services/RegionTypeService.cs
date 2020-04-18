using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmApp.Infrastructure.Data.Contexts;

namespace FarmAppServer.Services
{
    public interface IRegionTypeService
    {
        IQueryable SearchRegionType(string param);
    }
    public class RegionTypeService : IRegionTypeService
    {
        private readonly FarmAppContext _context;

        public RegionTypeService(FarmAppContext context)
        {
            _context = context;
        }
        public IQueryable SearchRegionType(string param)
        {
            var regionType = _context.RegionTypes.Where(x => x.RegionTypeName == param);
            return regionType;
        }
    }
}
