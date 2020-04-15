using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthentication.Models.ViewModels;

namespace UserAuthentication.Interfaces
{
    public interface IUsersService
    {
        Task<List<UserVm>> SearchUsers(string searchQuery);

        public Task<string> GetUserName(Guid id);

        public Task<UserDetailVm> GetUser(Guid id);
    }
}
