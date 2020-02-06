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
        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
            
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "idx"),
                new Claim("ManagR", "Cookie")
            };

            var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signingCredentails = new SigningCredentials(key, algorithm);
            
            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddHours(24),
                signingCredentails
                );

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
            if(user != null)
            {
               var result =  await _signInManager.CheckPasswordSignInAsync(user, loginInfo.Password, false);
               if (result.Succeeded)
                {
                    return Ok(new { access_token = tokenJson });
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
