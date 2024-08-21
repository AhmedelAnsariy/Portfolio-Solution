using AutoMapper;
using Portfolio.API.DTOS.Designs;
using Portfolio.Core.Models;

namespace Portfolio.API.Helper
{
    public class DesignUrlResolver : IValueResolver<Design, DesignToReturnDto, string>
    {
        private readonly IConfiguration _configuration;

        public DesignUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }





        public string Resolve(Design source, DesignToReturnDto destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["baseurl"]}{source.PictureUrl}";
            }
            return string.Empty;
        }
    }
}
