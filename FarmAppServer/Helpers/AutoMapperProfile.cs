using AutoMapper;
using FarmApp.Domain.Core.Entity;
using FarmAppServer.Models;

namespace FarmAppServer.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}