using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using EF_Models;
using EF_Models.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Maelstrom.Admin.Pages.SiteManager
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public DeleteModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Site Site { get; set; } = default!;
      public string? SiteImage { get; private set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Sites == null)
            {
                return NotFound();
            }

            var site = await _context.Sites.FirstOrDefaultAsync(m => m.SiteID == id);

            if (site == null)
            {
                return NotFound();
            }
            else 
            {
                Site = site;
                if (Site.ImageData != null && Site.ImageData.Length > 1 == true)
                {
                    var base64 = Convert.ToBase64String(Site.ImageData);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    SiteImage = imgSrc;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Sites == null)
            {
                return NotFound();
            }
            var site = await _context.Sites.FindAsync(id);

            if (site != null)
            {
                Site = site;
               
                _context.Sites.Remove(Site);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
