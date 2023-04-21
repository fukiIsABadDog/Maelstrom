using EF_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Maelstrom.Areas.Admin.Pages.ResultManager
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
            ViewData["SiteUserID"] = new SelectList(_context.SiteUsers, "SiteUserID", "SiteUserID");
            return Page();
        }

        [BindProperty]
        public TestResult TestResult { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.TestResults == null || TestResult == null)
            {
                return Page();
            }

            _context.TestResults.Add(TestResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
