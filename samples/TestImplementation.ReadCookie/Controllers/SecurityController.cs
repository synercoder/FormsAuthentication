using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Synercoding.FormsAuthentication;
using System.Threading.Tasks;

namespace TestImplementation.ReadCookie.Controllers
{
    [Authorize]
    public class SecurityController : Controller
    {
        private readonly FormsAuthenticationOptions _formsAuthenticationOptions;

        public SecurityController(IOptions<FormsAuthenticationOptions> options)
        {
            _formsAuthenticationOptions = options.Value;
        }

        public IActionResult Index()
        {
            var authCryptor = new FormsAuthenticationCryptor(_formsAuthenticationOptions);
            var ticket = authCryptor.Unprotect(Request.Cookies["TestCookie"]);
            ViewData["TestCookie-UserData"] = ticket.UserData;

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Index));
        }
    }
}
