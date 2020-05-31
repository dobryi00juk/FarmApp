using System;
using System.Collections.Generic;
using AutoMapper;
using FarmApp.Domain.Core.Entity;
using FarmAppServer.Models;
using FarmAppServer.Models.ApiMethods;
using FarmAppServer.Models.CodeAthTypes;
using FarmAppServer.Models.Drug;
using FarmAppServer.Models.Pharmacies;
using FarmAppServer.Models.Pharmacy;
using FarmAppServer.Models.Regions;
using FarmAppServer.Models.Roles;
using FarmAppServer.Models.Sales;
using FarmAppServer.Models.Users;
using FarmAppServer.Models.Vendors;
using static System.Decimal;

namespace FarmAppServer.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //users map
            CreateMap<Role, UserRoleDto>().ReverseMap();
            
            CreateMap<User, UserModelDto>()
                .ForMember(x => x.Role,
                    o => o.MapFrom(s => s.Role));
            
            CreateMap<User, AuthResponseDto>()
                .ForMember(x => x.Role,
                    o => o.MapFrom(s => s.Role));

            CreateMap<RegisterModelDto, User>()
                .ForMember(x => x.UserName,
                    o => o.MapFrom(s => s.FirstName + " " + s.LastName));
            CreateMap<UpdateModelDto, User>()
                .ForMember(x => x.UserName, 
                    o => o.MapFrom(s => s.FirstName + " " + s.LastName));

            //map for UserFilterByRole 
            CreateMap<User, UserFilterByRoleDto>()
                .ForMember(x => x.UserName, 
                    o => o.MapFrom(s => s.FirstName + " " + s.LastName));

            //vendor map
            CreateMap<Vendor, VendorDto>().ReverseMap();
            CreateMap<Vendor, PostVendorDto>().ReverseMap();
            CreateMap<Vendor, UpdateVendorDto>().ReverseMap();

            //regions
            CreateMap<Region, RegionDto>()
                .ForMember(x => x.Name,
                    o => o.MapFrom(s => s.RegionName))
                .ForMember(x => x.ParentId,
                    o => o.MapFrom(s => s.RegionId));
            CreateMap<Region, RegionSearchDto>()
                .ForMember(x => x.ChildRegion,
                    o =>
                        o.MapFrom(s => s.RegionId));
            
            CreateMap<Region, PostRegionDto>().ReverseMap();
            CreateMap<Region, UpdateRegionDto>().ReverseMap();

            //region types
            CreateMap<RegionType, RegionTypeDto>().ReverseMap();
            CreateMap<RegionType, PostRegionTypeDto>().ReverseMap();
            
            //roles
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, UpdateRoleDto>().ReverseMap();
            CreateMap<Role, PostRoleDto>().ReverseMap();

            //pharmacies
            CreateMap<Pharmacy, PharmacyDto>().ReverseMap();
            CreateMap<Pharmacy, PharmacyFilterDto>()
                .ForMember(x => x.ParentPharmacyName,
                    o => o.MapFrom(s => s.ParentPharmacy.PharmacyName))
                .ForMember(x => x.RegionName,
                    o => o.MapFrom(s => s.Region.RegionName));
            CreateMap<PharmacyFilterDto,Pharmacy>();
            CreateMap<PharmacyDto, Pharmacy>().ReverseMap();
            CreateMap<PostPharmacyDto, Pharmacy>().ReverseMap();

            //drugs
            CreateMap<Drug, DrugDto>().ReverseMap();

            //sales
            CreateMap<Sale, SaleDto>()
                .ForMember(x => x.DrugName,
                    o => o.MapFrom(s => s.Drug.DrugName))
                .ForMember(x => x.PharmacyName,
                    o => o.MapFrom(s => s.Pharmacy.PharmacyName))
                .ForMember(x => x.Amount,
                    o => o.MapFrom(s => Multiply(s.Price, s.Quantity)))
                .ForMember(x => x.SaleImportFileName,
                    o => o.MapFrom(s => s.SaleImportFile.FileName))
                .ReverseMap();

            CreateMap<Sale, UpdateSaleDto>();
            CreateMap<UpdateSaleDto, Sale>()
                .ForMember(x => x.Amount,
                    o => o.MapFrom(s => Multiply(s.Price, s.Quantity)))
                .ForMember(x => x.Price, o => o.Ignore());

            CreateMap<PostSaleDto, Sale>()
                .ForMember(x => x.Amount,
                    o => o.MapFrom(s => Multiply(s.Price, s.Quantity)))
                .ReverseMap();

            //CodeAthType
            CreateMap<CodeAthType, CodeAthTypeDto>()
                .ForMember(x => x.IdCodeAthId,
                    o => o.MapFrom(s => s.CodeAthId))
                .ForMember(x => x.ParentCode,
                    o => o.MapFrom(s => s.CodeAth.Code));

            CreateMap<PostCodeAthType, CodeAthType>()
                .ForMember(x => x.CodeAthId, 
                    o => o.MapFrom(s => s.ParentId));
           
            CreateMap<UpdateRegionDto, CodeAthType>().ReverseMap();

            //ApiMethods
            CreateMap<ApiMethod, ApiMethodDto>().ReverseMap();
            CreateMap<ApiMethodDto, UpdateApiMethodDto>().ReverseMap();
        }
    }
}