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
                    opt => opt.
                        MapFrom(c => c.Machines.Select(ms => ms.MachineId)));
            
            CreateMap<CategoryDto, Category>()
                .ForMember(m => m.Machines, 
                    opt => opt.Ignore());

            CreateMap<Log, Guid>().ConstructUsing(l => l.LogId);

            CreateMap<Log, LogDto>()
                .ForMember(m => m.Mechanic, 
                    opt => opt.MapFrom(m => m.Mechanic.Id))
                .ForMember(m => m.Machines,
                    opt => opt
                        .MapFrom(l => l.Machines.Select(ls => ls.MachineId)));
            
            CreateMap<LogDto, Log>()
                .ForMember(m => m.Mechanic, opt => opt.Ignore())
                .ForMember(m => m.Machines, opt => opt.Ignore());
        }
    }
}