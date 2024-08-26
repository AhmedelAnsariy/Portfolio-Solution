using Microsoft.AspNetCore.Identity;
using Portfolio.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Core.Services.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user , UserManager<AppUser> manager);



    }
}
