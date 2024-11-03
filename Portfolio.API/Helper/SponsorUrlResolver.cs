using AutoMapper;
using Portfolio.API.DTOS.Sponsor;
using Portfolio.Core.Models;

namespace Portfolio.API.Helper
{
    public class SponsorUrlResolver : IValueResolver<Sponspr, SponsorToReturnDTO, string>
    {


        private readonly IConfiguration _configuration;

        public SponsorUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public string Resolve(Sponspr source, SponsorToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["baseurl"]}{source.PictureUrl}";
            }
            return string.Empty;
        }


    }
}
