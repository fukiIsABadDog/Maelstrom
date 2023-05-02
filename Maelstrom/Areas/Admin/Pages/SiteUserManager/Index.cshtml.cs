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

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var siteuser = new SiteUser { IsAdmin = IsAdmin, SiteID = SiteId, SiteUserID = SiteUserId };
            SiteUser = await _context.SiteUsers.FindAsync(siteuser.SiteUserID);
            if (SiteUser == null)
            {
                Message = "There was an error finding that Site User";

                return Page();
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }
            Message = "Change Successful";
            return Page();
        }
        private bool SiteUserExists(int id)
        {
            return (_context.SiteUsers?.Any(e => e.SiteUserID == id)).GetValueOrDefault();
        }
    }
}
