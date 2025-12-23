using Microsoft.AspNetCore.Mvc;

namespace StrangerConnectMvc.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Room(string name)
        {
            ViewBag.NickName = name;
            return View();
        }
    }
}
