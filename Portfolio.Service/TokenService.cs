using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Core.Identity;
using Portfolio.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> manager)
        {
            var AuthClaims = new List<Claim>()
           {
               new Claim(ClaimTypes.GivenName , user.UserName),
               new Claim(ClaimTypes.Email,user.Email)
           };

            var userRoles = await manager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, role));
            }



            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));



            var token = new JwtSecurityToken(
                 issuer: _configuration["JWT:ValidIssure"],
                 audience: _configuration["JWT:ValidAudience"],
                 expires: DateTime.Now.AddDays(10),
                 claims: AuthClaims,
                 signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
                 );


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
