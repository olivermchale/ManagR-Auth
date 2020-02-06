using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserAuthentication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult SomeThing()
        {
            return Ok();
        }

        [Authorize]
        public IActionResult GetSecret()
        {
            return Ok();
        }

        public IActionResult Login()
        {
            return Ok();
        }
    }
}
