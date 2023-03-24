using Maelstrom.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom
{
    
    public class UserHomeModel : PageModel
    {
        private readonly MaelstromContext _context;

        public UserHomeModel(MaelstromContext context)
        {

            _context = context;
        }

        public string Message { get; private set; } = "PageModel in C#";

        public void OnGet()
        {
            if(User.Identity != null)
            {
                var currentUser = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                if (currentUser != null) 
                {
                    Message += $" The person loged in is: {currentUser.LastName}";
                }
                
            }
        }
           
    }
}
