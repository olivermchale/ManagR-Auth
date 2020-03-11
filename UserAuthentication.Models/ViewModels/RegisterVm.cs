using System;
using System.Collections.Generic;
using System.Text;
using UserAuthentication.Models.Types;

namespace UserAuthentication.Models.ViewModels
{
    public class RegisterVm
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ManagRRole Role { get; set; }
    }
}
