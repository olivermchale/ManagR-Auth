using System;
using System.Collections.Generic;
using System.Text;
using UserAuthentication.Models.Types;

namespace UserAuthentication.Models.ViewModels
{
    public class UserDetailVm
    {
        public ManagRRole Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
