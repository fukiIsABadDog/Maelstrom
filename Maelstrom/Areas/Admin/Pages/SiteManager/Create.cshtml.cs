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

namespace Maelstrom.Pages.SiteManager
{
    
    public class CreateModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;
    


        public CreateModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        public string Message { get; set; }

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty] //[UploadFileExtensions(Extensions = ".jpeg")]
        public IFormFile Upload { get; set; }


        public IActionResult OnGet()
        {
        ViewData["SiteTypeID"] = new SelectList(_context.SiteTypes, "SiteTypeID", "Name");
            return Page();
        }

        [BindProperty]
        public Site Site { get; set; } = default!;

        // could use this templete to validate input extension type.. but i have seen other ways
        //private string[] permittedExtensions = { ".txt", ".pdf" };

        // var ext = Path.GetExtension(uploadedFileName).ToLowerInvariant();

        //if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
        //{
        //    // The extension is invalid ... discontinue processing the file
        //}

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            using (var memoryStream = new MemoryStream())
            {
                await Upload.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 2097152)
                {

                    Site.ImageData = memoryStream.ToArray();
                    try
                    {
                        _context.Sites.Add(Site);
                        await _context.SaveChangesAsync();
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

              
                // having issues with this
                //if (!ModelState.IsValid)
                //{
                //    return Page();
                //}
               


            }

            return RedirectToPage("./Index");
        }
    }
}
