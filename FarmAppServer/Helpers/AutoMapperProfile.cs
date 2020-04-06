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
            CreateMap<Role, UserRoleDto>();
            
            //CreateMap<User, UserModelDto>();
            CreateMap<User, UserModelDto>()
                .ForMember(x => x.Role,
                    o => o.MapFrom(s => s.Role));
            //CreateMap<User, IEnumerable<UserModelDto>>();

            CreateMap<RegisterModelDto, User>();
            CreateMap<UpdateModelDto, User>();
        }
    }
}