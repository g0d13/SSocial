using System;
using System.Linq;
using AutoMapper;
using SSocial.Data;
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

            CreateMap<ApplicationUser, UserDetails>().ForMember(u => 
                u.UserId, opt => 
                opt.MapFrom(e => e.Id)).ReverseMap();

            // CreateMap<Log, Guid>().ConstructUsing(l => l.LogId);

            #region Request
            CreateMap<Request, RequestDto>()
                .ForMember(r => r.Machine, opt =>
                    opt.MapFrom(ops => ops.Machine.MachineId))
                .ForMember(r => r.Log,
                    opt => opt.MapFrom(ops => ops.Log.LogId))
                .ForMember(r => r.Supervisor,
                    opt => opt.MapFrom(op => op.Supervisor.Id));
            CreateMap<RequestDto, Request>()
                .ForMember(r => r.Log, opt => opt.Ignore())
                .ForMember(r => r.Supervisor, opt => opt.Ignore())
                .ForMember(r => r.Machine, opt => opt.Ignore());
            #endregion
            
        }
    }
}