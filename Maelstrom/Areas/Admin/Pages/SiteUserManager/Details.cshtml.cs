using EF_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.Admin.Pages.SiteUserManager
{
    public class DetailsModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public DetailsModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        public SiteUser SiteUser { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SiteUsers == null)
            {
                return NotFound();
            }

            var siteuser = await _context.SiteUsers.FirstOrDefaultAsync(m => m.SiteUserID == id);
            if (siteuser == null)
            {
                return NotFound();
            }
            else
            {
                SiteUser = siteuser;
            }
            return Page();
        }
    }
}
