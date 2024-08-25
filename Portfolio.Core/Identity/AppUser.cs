using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.Core.Identity
{
    public class AppUser : IdentityUser
    {
        public string? FName { get; set; }
        public int Age { get; set; }

    }
}
