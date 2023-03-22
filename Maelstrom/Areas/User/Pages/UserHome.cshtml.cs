using EF_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Maelstrom.Areas.User.Pages
{
    [Authorize]
    public class UserHomeModel : PageModel
    {
        private readonly MaelstromContext _context;

        // still deciding which route to go here or MVC
     

        public UserHomeModel(MaelstromContext context)
        {
            _context = context;
           
        }


        public void OnGet()
        {
           

        }

        public void OnPost() 
        {

        }


    }

}
