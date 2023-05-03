using EF_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.Admin.Pages.SiteUserManager
{
    public class IndexModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public IndexModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }


        public IList<SiteUser> SiteUsers { get; set; } = default!;

        public SiteUser SiteUser { get; set; }
        [BindProperty]
        public int SiteUserId { get; set; }
        [BindProperty]
        public int SiteId { get; set; }
        [BindProperty]
        public bool IsAdmin { get; set; }
        public String Message { get; set; }
        public async Task OnGetAsync()
        {
            if (_context.SiteUsers != null)
            {
                SiteUsers = await _context.SiteUsers
                .Include(s => s.Site).Include(a => a.AppUser).ToListAsync();
            }

        }
    
    }
}
