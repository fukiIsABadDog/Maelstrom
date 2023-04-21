using EF_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.Admin.Pages.ResultManager
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public DetailsModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

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
    }
}
