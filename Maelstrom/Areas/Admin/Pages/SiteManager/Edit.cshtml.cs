using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF_Models;
using EF_Models.Models;

namespace Maelstrom.Pages.SiteManager
{
    public class EditModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;

        public EditModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Site Site { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Sites == null)
            {
                return NotFound();
            }

            var site =  await _context.Sites.FirstOrDefaultAsync(m => m.SiteID == id);
            if (site == null)
            {
                return NotFound();
            }
            Site = site;
           ViewData["SiteTypeID"] = new SelectList(_context.SiteTypes, "SiteTypeID", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Site).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(Site.SiteID))
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

        private bool SiteExists(int id)
        {
          return (_context.Sites?.Any(e => e.SiteID == id)).GetValueOrDefault();
        }
    }
}
