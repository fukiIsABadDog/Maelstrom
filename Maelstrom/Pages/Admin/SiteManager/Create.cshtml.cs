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

namespace Maelstrom.Pages.SiteManager
{
    public class CreateModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;
        private readonly IWebHostEnvironment _environment;


        public CreateModel(EF_Models.MaelstromContext context, IWebHostEnvironment environment )
        {
            _context = context;
            _environment = environment;

        }

        [BindProperty]
        [Required]
        public string Name { get; set; }

        [BindProperty]
        [UploadFileExtensions(Extensions = ".jpg")]
        public IFormFile Upload { get; set; }
        [TempData]
        public string Photo { get; set; }

        public IActionResult OnGet()
        {
        ViewData["SiteTypeID"] = new SelectList(_context.SiteTypes, "SiteTypeID", "Name");
            return Page();
        }

        [BindProperty]
        public Site Site { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Sites == null || Site == null)
            {
                return Page();
            }

            _context.Sites.Add(Site);
            await _context.SaveChangesAsync();

            TempData["Name"] = Name;
            Photo = $"{Name.ToLower()}{Path.GetExtension(Upload.FileName)}"; //need to make tank name unique
            var filePath = Path.Combine(_environment.WebRootPath, "media", "userImages", Photo);
            using var stream = System.IO.File.Create(filePath);
            await Upload.CopyToAsync(stream);

            return RedirectToPage("./Index");
        }
    }
}
