using AutoMapper;
using Portfolio.API.DTOS.Category;
using Portfolio.API.DTOS.Client;
using Portfolio.API.DTOS.Contact;
using Portfolio.API.DTOS.Designs;
using Portfolio.API.DTOS.Team;
using Portfolio.Core.Models;

namespace Portfolio.API.Helper
{
    public class MappingProfiles : Profile
    {


        public MappingProfiles()
        {
            CreateMap<Design, DesignToReturnDto>()
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                 .ForMember(d => d.PictureUrl, o => o.MapFrom<DesignUrlResolver>());



            CreateMap<ClientReview, ClientToReturnDTO>()
                .ForMember(cl => cl.PictureUrl, o => o.MapFrom<ClientUrlResolver>());



            CreateMap<ClientToCreateDTO, ClientReview>()
            .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());


            CreateMap<CategoryToAddDTO, Category>();
            CreateMap<CategoryToReturnDto, Category>().ReverseMap();




            CreateMap<DesignToAddDTO,Design>()
                .ForMember(dest =>dest.PictureUrl, opt => opt.Ignore());




            CreateMap<ContactDTO,Contact>().ReverseMap();
            CreateMap<Contact, ContactToReturnDTO>();



            CreateMap<TeamMember, TeamToReturnDTO>()
                .ForMember(t => t.PictureUrl, o => o.MapFrom<TeamUrlResolver>());


            CreateMap<TeamToCreateDTO, TeamMember>()
           .ForMember(dest => dest.PictureUrl, opt => opt.Ignore());










        }



    }
}
