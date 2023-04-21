using EF_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.Admin.Pages.ResultManager
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public IndexModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        public IList<TestResult> TestResult { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.TestResults != null)
            {
                TestResult = await _context.TestResults
                .Include(t => t.SiteUser).ToListAsync();
            }
        }
    }
}
