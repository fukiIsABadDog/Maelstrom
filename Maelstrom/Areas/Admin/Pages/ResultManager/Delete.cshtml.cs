using EF_Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.Admin.Pages.ResultManager
{
    public class DeleteModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public DeleteModel(EF_Models.MaelstromContext context)
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
            else
            {
                TestResult = testresult;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.TestResults == null)
            {
                return NotFound();
            }
            var testresult = await _context.TestResults.FindAsync(id);

            if (testresult != null)
            {
                TestResult = testresult;
                _context.TestResults.Remove(TestResult);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
