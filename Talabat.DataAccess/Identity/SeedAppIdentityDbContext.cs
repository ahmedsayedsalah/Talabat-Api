using Talabat.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.DataAccess.Identity
{
    public static class SeedAppIdentityDbContext
    {
        public static async Task SeedUserAsyc(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName= "Ahmed Sayed",
                    UserName= "ahmed.sayed",
                    Email= "aahme99966@gmail.com",
                    PhoneNumber= "01234567890"
                };

               await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
