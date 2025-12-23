using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StrangerConnectMvc.Models;

namespace StrangerConnectMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Join(string nickname)
        {
            return RedirectToAction("Room", "Chat", new { name = nickname });
        }
    }
}
