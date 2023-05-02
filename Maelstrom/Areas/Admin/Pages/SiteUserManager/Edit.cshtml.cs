using EF_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.Admin.Pages.SiteUserManager
{
    public class EditModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public EditModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        [BindProperty]
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
            SiteUser = siteuser;
            ViewData["SiteID"] = new SelectList(_context.Sites, "SiteID", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SiteUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteUserExists(SiteUser.SiteUserID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SiteUserExists(int id)
        {
            return (_context.SiteUsers?.Any(e => e.SiteUserID == id)).GetValueOrDefault();
        }
    }
}
