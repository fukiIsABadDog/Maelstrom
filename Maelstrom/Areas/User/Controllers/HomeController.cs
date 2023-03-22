using Microsoft.AspNetCore.Mvc;

namespace Maelstrom.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        private readonly MaelstromContext _context;

        public HomeController(MaelstromContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //if(User.Identity.IsAuthenticated)
            //{
            //    var result = _context.Users.FirstOrDefault(x => x.LastName == User.Identity.Name);
            //    return View(result);
            //}
            
            return View();
        }
    }
}
