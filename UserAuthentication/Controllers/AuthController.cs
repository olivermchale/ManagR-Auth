using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
    [EnableCors("ManagRAppServices")]
    public class AuthController : Controller
    {
        private readonly UserManager<ManagRUser> _userManager;
        private readonly SignInManager<ManagRUser> _signInManager;
        private readonly IIDAuthService _idAuthService;
        public AuthController(UserManager<ManagRUser> userManager, SignInManager<ManagRUser> signInManager, IIDAuthService idAuthService)
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
            var user = new ManagRUser
            {
                UserName = registerInfo.Username,
                Role = registerInfo.Role,
                Email = registerInfo.Email,
                FirstName = registerInfo.FirstName,
                LastName = registerInfo.LastName
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
