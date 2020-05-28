using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using FarmAppServer.Helpers;
using FarmAppServer.Models.Drug;

namespace FarmAppServer.Services
{
    public interface IDrugService
    {
        Task<Drug> PostDrug(Drug drug);
    }
    public class DrugService : IDrugService
    {
        private readonly FarmAppContext _context;

        public DrugService(FarmAppContext context)
        {
            _context = context;
        }
        public async Task<Drug> PostDrug(Drug drug)
        {
            if (drug == null) throw new ArgumentNullException(nameof(drug));

            if(_context.Drugs.Any(x => x.CodeAthType == drug.CodeAthType & x.IsDeleted == false 
                                       || drug.CodeAthTypeId == x.CodeAthTypeId
                                       || drug.DrugName == x.DrugName))
                throw new AppException("PharmacyName \"" + drug.CodeAthType + "\" is already taken");

            await _context.Drugs.AddAsync(drug);
            await _context.SaveChangesAsync();

            return drug;
        }
    }
}
