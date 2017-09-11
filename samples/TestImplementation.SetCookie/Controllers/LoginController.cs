using System.Web.Mvc;
using TestImplementation.SetCookie.Models;

namespace TestImplementation.SetCookie.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index(string returnUrl)
        {
            var model = new LoginVM()
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                System.Web.Security.FormsAuthentication.SetAuthCookie(model.UserName, true);
                return Redirect(model.ReturnUrl);
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return RedirectToAction(nameof(Index));
        }
    }
}