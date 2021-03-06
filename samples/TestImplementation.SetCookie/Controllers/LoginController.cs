using System;
using System.Web;
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
                var ticket = new System.Web.Security.FormsAuthenticationTicket(1, "TestTicket", DateTime.Now, DateTime.Now.AddDays(1), true, "The answer is '42'.");
                var encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie("TestCookie", encryptedTicket));

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