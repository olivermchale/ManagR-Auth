using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using UserAuthentication.Models.ViewModels;

namespace UserAuthentication.Data
{
    public class UserAuthenticationDb : IdentityDbContext<ManagRUser>
    {
        public UserAuthenticationDb(DbContextOptions<UserAuthenticationDb> options)
            : base(options)
        {

        }
    }
}
