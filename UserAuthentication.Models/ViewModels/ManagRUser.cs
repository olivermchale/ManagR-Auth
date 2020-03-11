using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using UserAuthentication.Models.Types;

namespace UserAuthentication.Models.ViewModels
{
    public class ManagRUser : IdentityUser
    {
        public ManagRRole Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
