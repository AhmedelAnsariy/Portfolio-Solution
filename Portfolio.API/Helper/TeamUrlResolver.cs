using AutoMapper;
using Portfolio.API.DTOS.Team;
using Portfolio.Core.Models;

namespace Portfolio.API.Helper
{
    public class TeamUrlResolver : IValueResolver<TeamMember, TeamToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public TeamUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(TeamMember source, TeamToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["baseurl"]}{source.PictureUrl}";
            }
            return string.Empty;
        }



    }


}
