using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class ExampleUserPrincipalFactory:IUserClaimsPrincipalFactory<IdentityUser>
    {
        public Task<ClaimsPrincipal> CreateAsync(IdentityUser user)
        {
            ClaimsIdentity identity = new ClaimsIdentity("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            return Task.FromResult(principal);
        }
    }
}
