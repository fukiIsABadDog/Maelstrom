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
    public class DetailsModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public DetailsModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

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
                //this 100% needs a service method
                if (Site.ImageData != null && Site.ImageData.Length > 1 == true)
                {
                    var base64 = Convert.ToBase64String(Site.ImageData);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    SiteImage = imgSrc;
                }
            }
            return Page();
        }
    }
}
