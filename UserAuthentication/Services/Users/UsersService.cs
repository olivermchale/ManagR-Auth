using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserAuthentication.Interfaces;
using UserAuthentication.Models.ViewModels;

namespace UserAuthentication.Services.Users
{
    public class UsersService : IUsersService
    {
        private UserManager<ManagRUser> _userManager;
        public UsersService(UserManager<ManagRUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<UserVm>> SearchUsers(string searchQuery)
        {
            var users = await _userManager.Users
                    .Where(u => u.FirstName.Contains(searchQuery)
                            || u.LastName.Contains(searchQuery)
                            || u.Id.Contains(searchQuery))
                    .Take(5).Select(u => new UserVm()
                    {
                        Id = Guid.Parse(u.Id),
                        Name = u.FirstName + " " + u.LastName
                    }).ToListAsync();

            return users;
        }
    }
}
