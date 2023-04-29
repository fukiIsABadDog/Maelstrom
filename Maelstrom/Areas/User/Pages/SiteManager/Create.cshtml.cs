using EF_Models.Models;
using Maelstrom.ValidationAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.SiteManager
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;
        public CreateModel(EF_Models.MaelstromContext context)
        {
            _context = context;
        }

        public string Message { get; set; }

        [BindProperty]
        [UploadFileExtensions(Extensions = ".jpeg,.jpg")]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public Site Site { get; set; } = default!;
        public SiteUser SiteUser { get; set; }
        public IIdentity CurrentUser { get; set; }
        public AppUser AppUser { get; set; }
        public IActionResult OnGet()
        {
            ViewData["SiteTypeID"] = new SelectList(_context.SiteTypes, "SiteTypeID", "Name");
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            CurrentUser = User.Identity!;
            var appUser = await _context.AppUsers.Where(x => x.Email == CurrentUser.Name).FirstAsync();

            using (var memoryStream = new MemoryStream())
            {
                await Upload.CopyToAsync(memoryStream);

                // Upload the file if less than 2 MB
                if (memoryStream.Length < 4194304)
                {
                    Site.ImageData = memoryStream.ToArray();
                    try
                    {
                        if (ModelState.IsValid && appUser != null)
                        {
                            this.SiteUser = new SiteUser { AppUser = appUser, Site = this.Site }; // needs to be tested - 4/29 1:30
                            _context.Sites.Add(Site);
                            _context.SiteUsers.Add(SiteUser);
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
                    ModelState.AddModelError("Upload", "That file is too large. It should be under 4MB.");
                    return Page();
                }
            }
            return RedirectToPage("./Index");
        }
    }
}
