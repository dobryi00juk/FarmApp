using System.Collections.Generic;
using AutoMapper;
using FarmApp.Domain.Core.Entity;
using FarmAppServer.Models;
using FarmAppServer.Models.Users;

namespace FarmAppServer.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //users map
            CreateMap<Role, UserRoleDto>();
            
            CreateMap<User, UserModelDto>()
                .ForMember(x => x.Role,
                    o => o.MapFrom(s => s.Role));
            
            CreateMap<User, AuthResponseDto>()
                .ForMember(x => x.Role,
                    o => o.MapFrom(s => s.Role));

            CreateMap<RegisterModelDto, User>()
                .ForMember(x => x.UserName,
                    o => o.MapFrom(s => s.FirstName + " " + s.LastName));
            CreateMap<UpdateModelDto, User>();

            //map for UserFilterByRole 
            CreateMap<User, UserFilterByRoleDto>()
                .ForMember(x => x.UserName, 
                    o => o.MapFrom(s => s.FirstName + " " + s.LastName));

            //vendor map
            CreateMap<Vendor, VendorDto>();
            CreateMap<VendorDto, Vendor>();

            //regions
            CreateMap<Region, RegionDto>()
                .ForMember(x => x.ParentRegionName,
                    o => 
                        o.MapFrom(s => s.ParentRegion.RegionName))
                .ForMember(x => x.RegionTypeId,
                    o => 
                        o.MapFrom(s => s.RegionId))
                .ForMember(x => x.RegionTypeName,
                    o => 
                        o.MapFrom(s => s.RegionType.RegionTypeName));
            
            //region types
            CreateMap<RegionType, RegionTypeDto>();
            CreateMap<RegionTypeDto, RegionType>();
            
            //roles
            CreateMap<Role, RoleDto>();
        }
    }
}