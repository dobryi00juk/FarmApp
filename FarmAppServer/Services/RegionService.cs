using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;

namespace FarmAppServer.Services
{
    public interface IRegionService
    {
        IQueryable RegionNameSearch(string param);
    }
    public class RegionService : IRegionService
    {
        private readonly FarmAppContext _context;

        public RegionService(FarmAppContext context)
        {
            _context = context;
        }
        
        //Фильтры: TextBox -> RegionName
        public IQueryable RegionNameSearch(string param)
        {
            var regions = _context.Regions.Where(x => 
                x.RegionName
                    .ToLower()
                    .Contains(param.ToLower()));
            
            return regions;
        }
        
        //CheckBoxCombobox(Мультивыбор) -> ParentRegionName
        // public IQueryable ParentRegionNameFilter(string[] parentRegionNames)
        // {
        //     
        // }
    }
}
