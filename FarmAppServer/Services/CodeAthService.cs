using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FarmApp.Domain.Core.Entity;
using FarmApp.Infrastructure.Data.Contexts;
using FarmAppServer.Helpers;
using FarmAppServer.Models.CodeAthTypes;
using FarmAppServer.Models.Sales;
using Microsoft.EntityFrameworkCore;

namespace FarmAppServer.Services
{
    public interface ICodeAthService
    {
        Task<CodeAthType> PostCodeAthTypeAsync(CodeAthType sale);
        Task<CodeAthTypeDto> GetCodeAthTypeById(int id);
        IQueryable<CodeAthTypeDto> GetCodeAthTypes();
        Task<bool> DeleteCodeAthTypeAsync(int id);
    }
    public class CodeAthService : ICodeAthService
    {
        private readonly FarmAppContext _context;
        private readonly IMapper _mapper;

        public CodeAthService(FarmAppContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<CodeAthType> PostCodeAthTypeAsync(CodeAthType codeAthType)
        {
            if (codeAthType == null) throw new ArgumentNullException(nameof(codeAthType));

            if (_context.Sales.Any(x => x.Id == codeAthType.Id & x.IsDeleted == false))
                throw new AppException("CodeAthType \"" + codeAthType.Id + "\" is already taken");

            _context.CodeAthTypes.Add(codeAthType);
            await _context.SaveChangesAsync();

            return codeAthType;
        }

        public async Task<CodeAthTypeDto> GetCodeAthTypeById(int id)
        {
            var codeAthType = _context.CodeAthTypes.Where(x => x.Id == id && x.IsDeleted == false);

            if (codeAthType == null)
                throw new AppException("Sale not found!");

            var result = await _mapper.ProjectTo<CodeAthTypeDto>(codeAthType).FirstOrDefaultAsync();

            return result;
        }

        public IQueryable<CodeAthTypeDto> GetCodeAthTypes()
        {
            var codeAthTypes = _context.CodeAthTypes;

            if (codeAthTypes == null)
                throw new AppException("CodeAthType not found!");

            var result = _mapper.ProjectTo<CodeAthTypeDto>(codeAthTypes);

            return result;
        }

        public Task<bool> DeleteCodeAthTypeAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
