using EF_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Admin.Pages.SiteManager
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public IndexModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        public IList<Site> Site { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Sites != null)
            {
                Site = await _context.Sites
                .Include(s => s.SiteType).ToListAsync();
            }
        }
    }
}
