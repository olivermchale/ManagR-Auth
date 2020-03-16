using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserAuthentication.Interfaces;
using UserAuthentication.Models.ViewModels;

namespace UserAuthentication.Controllers
{
    public class UsersController : Controller
    {
        private IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }
        public async Task<IActionResult> SearchForUser(string searchQuery)
        {
            var users = await _usersService.SearchUsers(searchQuery);
            return Ok(users);
        }
    }
}