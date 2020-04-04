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
            CreateMap<Role, UserRoleDto>()
                .ForMember(x => x.RoleName,
                    o => o.MapFrom(s => s.RoleName));

            CreateMap<User, UserModelDto>()
                .ForMember(x => x.Role,
                    o => o.MapFrom(s => s.Role));
            
            CreateMap<RegisterModelDto, User>();
            CreateMap<UpdateModelDto, User>();
        }
    }
}