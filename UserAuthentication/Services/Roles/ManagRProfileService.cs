using IdentityModel;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserAuthentication.Models.Types;
using UserAuthentication.Models.ViewModels;

namespace UserAuthentication
{
    public class ManagRProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ManagRUser> _claimsFactory;
        private readonly UserManager<ManagRUser> _userManager;

        public ManagRProfileService(IUserClaimsPrincipalFactory<ManagRUser> claimsFactory, UserManager<ManagRUser> userManager)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(subjectId);
            var principal = await _claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.UserName));

            if(user.Role == ManagRRole.Analyst)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "analyst"));
                claims.Add(new Claim(JwtClaimTypes.Role, "leader"));
                claims.Add(new Claim(JwtClaimTypes.Role, "user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "spectator"));
            }
            else if (user.Role == ManagRRole.Leader)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "leader"));
                claims.Add(new Claim(JwtClaimTypes.Role, "user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "spectator"));
            }
            else if (user.Role == ManagRRole.User)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "user"));
                claims.Add(new Claim(JwtClaimTypes.Role, "spectator"));
            } else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "spectator"));
            }

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(subjectId);
            context.IsActive = user != null;
        }
    }
}
