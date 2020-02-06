using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace UserAuthentication.Data
{
    public class UserAuthenticationDb : IdentityDbContext
    {
        public UserAuthenticationDb(DbContextOptions<UserAuthenticationDb> options)
            : base(options)
        {

        }
    }
}
