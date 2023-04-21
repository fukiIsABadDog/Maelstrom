using EF_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.Admin.Pages.ResultManager
{
    public class EditModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public EditModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TestResult TestResult { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TestResults == null)
            {
                return NotFound();
            }

            var testresult = await _context.TestResults.FirstOrDefaultAsync(m => m.TestResultID == id);
            if (testresult == null)
            {
                return NotFound();
            }
            TestResult = testresult;
            ViewData["SiteUserID"] = new SelectList(_context.SiteUsers, "SiteUserID", "SiteUserID");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TestResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestResultExists(TestResult.TestResultID))
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

        private bool TestResultExists(int id)
        {
            return (_context.TestResults?.Any(e => e.TestResultID == id)).GetValueOrDefault();
        }
    }
}
