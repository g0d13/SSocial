using System;
using System.Linq;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;

namespace SSocial.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Origin destiny
            CreateMap<RegisterUserDto, User>()
                .ForMember(e => e.UserName, opt => opt.MapFrom(c => c.Email));
            CreateMap<User, RegisterUserDto>()
                .ForMember(m => m.Role, opt => opt.Ignore());
            CreateMap<User, UserDetails>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();

            CreateMap<Log, LogDto>().ReverseMap();
        }
    }
}