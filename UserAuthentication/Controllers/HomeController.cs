using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserAuthentication.Models.ViewModels;

namespace UserAuthentication.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IIDAuthService _idAuthService;
        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IIDAuthService idAuthService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _idAuthService = idAuthService;
        }
        public IActionResult SomeThing()
        {
            return Ok();
        }

        [Authorize]
        public IActionResult GetSecret()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginVm loginInfo)
        {
            var user = await _userManager.FindByNameAsync(loginInfo.Username);
            if(user != null)
            {
               var result =  await _signInManager.CheckPasswordSignInAsync(user, loginInfo.Password, false);
               if (result.Succeeded)
                {
                    var token = await _idAuthService.Authenticate(loginInfo);
                    return Ok(new { access_token = token });
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterVm registerInfo)
        {
            var user = new IdentityUser
            {
                UserName = registerInfo.Username,
                Email = "",
            };

            var result = await _userManager.CreateAsync(user, registerInfo.Password);

            if (result.Succeeded)
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            return Ok(2);
        }
    }
}
