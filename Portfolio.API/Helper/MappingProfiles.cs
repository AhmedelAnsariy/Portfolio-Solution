using AutoMapper;
using Portfolio.API.DTOS.Designs;
using Portfolio.Core.Models;

namespace Portfolio.API.Helper
{
    public class MappingProfiles : Profile
    {


        public MappingProfiles()
        {
            CreateMap<Design, DesignToReturnDto>()
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name));
        }

    }
}
