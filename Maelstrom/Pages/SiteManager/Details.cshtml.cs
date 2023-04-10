﻿using System;
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
            }
            return Page();
        }
    }
}
