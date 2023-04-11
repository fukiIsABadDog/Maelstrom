using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EF_Models;
using EF_Models.Models;

namespace Maelstrom.Pages.SiteManager
{
    public class IndexModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public IndexModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        public IList<Site> Site { get;set; } = default!;

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
