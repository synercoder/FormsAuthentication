using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestImplementation.ReadCookie.Models;

namespace TestImplementation.ReadCookie.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
