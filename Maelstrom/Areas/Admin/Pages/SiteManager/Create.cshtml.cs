using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EF_Models;
using EF_Models.Models;
using Microsoft.Extensions.Hosting;
using Maelstrom.ValidationAttributes;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Maelstrom.Pages.SiteManager
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;
    


        public CreateModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        public string Message { get; set; }


        [BindProperty] //[UploadFileExtensions(Extensions = ".jpeg")]
        public IFormFile Upload { get; set; }

        [BindProperty]
        public Site Site { get; set; } = default!;

        public IActionResult OnGet()
        {
            ViewData["SiteTypeID"] = new SelectList(_context.SiteTypes, "SiteTypeID", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            using (var memoryStream = new MemoryStream())
            {
                await Upload.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {

                    Site.ImageData = memoryStream.ToArray();

                    if (!ModelState.IsValid)
                    {
                        var message = string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
                        
                    }


                    try
                    {
                        if (ModelState.IsValid) 
                        {
                            _context.Sites.Add(Site);
                            await _context.SaveChangesAsync();
                        }
                        
                    }
                    catch
                    {
                        Message = "There was an issue saving the data as entered.";   
                    }   
                }
                else
                {
                    ModelState.AddModelError("File", "The file is too large.");
                }
               
            }

            return RedirectToPage("./Index");
        }
    }
}
