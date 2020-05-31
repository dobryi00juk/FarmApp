using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using FarmAppServer.Helpers;
using FarmAppServer.Models;
using FarmAppServer.Models.Regions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmAppServer.Services
{
    public interface IRegionService
    {
        IQueryable RegionNameSearch(string param);
        Task<Region> PostRegion(Region region);
        Task<bool> DeleteRegionAsync(int id);
        Task<bool> UpdateRegionAsync(int id, UpdateRegionDto model);
    }
    public class RegionService : IRegionService
    {
        private readonly FarmAppContext _context;
        private readonly IMapper _mapper;

        public RegionService(FarmAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        //Post region
        public async Task<Region> PostRegion(Region region)
        {
            if (region == null) throw new ArgumentNullException(nameof(region));

            if(_context.Regions.Any(x => x.RegionName == region.RegionName & x.IsDeleted == false))
                throw new AppException("RegionName \"" + region.RegionName + "\" is already taken");

            _context.Regions.Add(region);
            await _context.SaveChangesAsync();

            return region;
        }

        public async Task<bool> DeleteRegionAsync(int id)
        {
            var region = await _context.Regions.FindAsync(id);

            if (region == null || region.IsDeleted == true) return false;

            region.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateRegionAsync(int id, UpdateRegionDto model)
        {
            var region = await _context.Regions.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();

            if (region == null) return false;

            _mapper.Map(model, region);
            _context.Update(region);
            var updated = await _context.SaveChangesAsync();

            return updated > 0;
        }

        //Фильтры: TextBox -> RegionName
        public IQueryable RegionNameSearch(string param)
        {
            if (string.IsNullOrEmpty(param))
                throw new ArgumentException("Value cannot be null or empty.", nameof(param));

            var request = param.ToUpper();

            var regions = _context.Regions.Where(x => x.RegionName.ToUpper().Contains(request) ||
                                                      x.Population.ToString().Contains(request));// ||
                                                      //x.ParentRegion.RegionName.ToUpper().Contains(request));
            return regions;
        }
    }
}
