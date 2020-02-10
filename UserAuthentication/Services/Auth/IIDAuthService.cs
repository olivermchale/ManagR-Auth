using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthentication.Models.ViewModels;

namespace UserAuthentication
{
    public interface IIDAuthService
    {
        Task<string> Authenticate(LoginVm loginInfo);
    }
}
