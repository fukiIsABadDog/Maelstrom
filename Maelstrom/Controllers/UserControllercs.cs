using Microsoft.AspNetCore.Mvc;

namespace Maelstrom.Controllers
{
    public class UserControllercs : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
