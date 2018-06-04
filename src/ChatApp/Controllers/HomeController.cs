using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChatApp.Controllers
{
    /// <summary>
    /// Landing page controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Only entry point for SPA application
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Error entry for application
        /// </summary>
        public IActionResult Error()
        {
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View();
        }
    }
}
