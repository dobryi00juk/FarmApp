using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using FarmAppServer.Helpers;
using FarmAppServer.Models.Pharmacy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmAppServer.Services
{
    public interface IPharmacyService
    {
        Task<Pharmacy> PostPharmacyAsync(Pharmacy pharmacy);
        void UpdatePharmacyAsync(Pharmacy modelPharmacy);
        Task<bool> DeletePharmacyAsync(int id);
        IQueryable SearchPharmacy(string pharmacyName);
    }
    public class PharmacyService : IPharmacyService
    {
        private readonly FarmAppContext _context;

        public PharmacyService(FarmAppContext context)
        {
            _context = context;
        }

        public async Task<Pharmacy> PostPharmacyAsync(Pharmacy pharmacy)
        {
            if (pharmacy == null) throw new ArgumentNullException(nameof(pharmacy));

            if (_context.Pharmacies.Any(x => x.PharmacyName == pharmacy.PharmacyName & x.IsDeleted == false))
                throw new AppException("PharmacyName \"" + pharmacy.PharmacyName + "\" is already taken");

            if (!CheckForeignKey(pharmacy))
                throw new AppException("Region not found");
            
            await _context.Pharmacies.AddAsync(pharmacy);
            await _context.SaveChangesAsync();
    
            return pharmacy;
        }

        public void UpdatePharmacyAsync(Pharmacy modelPharmacy)
        {
            var pharmacy =
                _context.Pharmacies.FirstOrDefault(x => x.Id == modelPharmacy.Id && x.IsDeleted == false);

            if (pharmacy == null)
                throw new AppException("Pharmacy not found");

            if (!string.IsNullOrWhiteSpace(modelPharmacy.PharmacyName) &&
                modelPharmacy.PharmacyName != pharmacy.PharmacyName)
            {
                if (_context.Pharmacies.Any(x => x.PharmacyName == modelPharmacy.PharmacyName))
                    throw new AppException("PharmacyName " + modelPharmacy.PharmacyName + " is already taken");

                pharmacy.PharmacyName = modelPharmacy.PharmacyName;
            }

            //update
            if (!string.IsNullOrWhiteSpace(modelPharmacy.PharmacyName)) pharmacy.PharmacyName = modelPharmacy.PharmacyName;
            pharmacy.IsMode = modelPharmacy.IsMode;
            pharmacy.IsNetwork = modelPharmacy.IsNetwork;
            pharmacy.IsType = modelPharmacy.IsType;
            pharmacy.PharmacyId = modelPharmacy.PharmacyId;
            pharmacy.RegionId = modelPharmacy.RegionId;

            _context.Entry(pharmacy).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<bool> DeletePharmacyAsync(int id)
        {
            var pharmacy = await _context.Pharmacies.FindAsync(id);

            if (pharmacy == null || pharmacy.IsDeleted == true) return false;

            pharmacy.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        //TextBox -> PharmacyName
        public IQueryable SearchPharmacy(string pharmacyName)
        {
            if (string.IsNullOrEmpty(pharmacyName))
                throw new ArgumentException("Value cannot be null or empty.", nameof(pharmacyName));

            var pharmacy = _context.Pharmacies.Where(x => x.PharmacyName.ToLower()
                .Contains(pharmacyName.ToLower()));

            return pharmacy;
        }

        public bool CheckForeignKey(Pharmacy pharmacy)
        {
            return _context.Regions.Any(x => x.Id == pharmacy.RegionId);
        }

    }
}