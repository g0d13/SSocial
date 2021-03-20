using System;
using System.Linq;
using AutoMapper;
using SSocial.Dtos;
using SSocial.Models;

namespace SSocial.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Origin destiny
            CreateMap<Category, Guid>().ConstructUsing(x => x.CategoryId);
            
            CreateMap<Category, CategoryDto>()
                .ForMember(m => m.Machines,
                    opt => opt.MapFrom(c => 
                        c.Machines.Select(ms => ms.MachineId)));
            
            CreateMap<CategoryDto, Category>()
                .ForMember(m => m.Machines, 
                    opt => opt.Ignore());
        }
    }
}