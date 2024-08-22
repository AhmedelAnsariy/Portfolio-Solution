using AutoMapper;
using Portfolio.API.DTOS.Client;
using Portfolio.Core.Models;

namespace Portfolio.API.Helper
{
    public class ClientUrlResolver : IValueResolver<ClientReview, ClientToReturnDTO, string>
    {

        private readonly IConfiguration _configuration;

        public ClientUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }





        public string Resolve(ClientReview source, ClientToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["baseurl"]}{source.PictureUrl}";
            }
            return string.Empty;
        }




    }
}
