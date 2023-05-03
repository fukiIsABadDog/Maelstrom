using EF_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Maelstrom.Areas.Admin.Pages.SiteUserManager
{
    public class CreateModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;
        public CreateModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            ViewData["SiteID"] = new SelectList(_context.Sites, "SiteID", "Name");
            return Page();
        }
        [BindProperty]
        public SiteUser SiteUser { get; set; } = default!;
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.SiteUsers == null || SiteUser == null)
            {
                return Page();
            }
            _context.SiteUsers.Add(SiteUser);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
