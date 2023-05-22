using EF_Models.Models;
using Maelstrom.Services;
using Maelstrom.ValidationAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security.Principal;
namespace Maelstrom.Areas.User.Pages.SiteManager
{
    [Authorize]
    public class CreateModel : PageModel
    {
       private readonly AppUserService _appUserService;
        public CreateModel(AppUserService appUserService)
        {
            _appUserService = appUserService;
        }


        public IIdentity LoggedInUser { get; set; }
        public Site Site { get; set; } = default!;
        public SiteUser SiteUser { get; set; }

        [DisplayName("Upload Image")]
        [UploadFileExtensions(Extensions = ".jpeg,.jpg")]
        public IFormFile? Upload { get; set; }
        [BindProperty]
        public AppUser AppUser { get; set; }
        public string Message { get; set; }


        public IActionResult OnGet()
        {
            ViewData["SiteTypeID"] = _appUserService.GetAllSiteTypes(); 
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            LoggedInUser = User.Identity!;
            var appUser = await _appUserService.FindAppUser(LoggedInUser);
            if (Upload == null)
            {
                Site.ImageData = null;
            }
            else
            {
                using (var memoryStream = new MemoryStream()) // turn into service
                {
                    await Upload.CopyToAsync(memoryStream);
                    if (memoryStream.Length < 4194304)
                    {
                        Site.ImageData = memoryStream.ToArray();
                    }
                    else
                    {
                        ModelState.AddModelError("Upload", "That file is too large. It should be under 4MB.");
                        ViewData["SiteTypeID"] = new SelectList(_context.SiteTypes, "SiteTypeID", "Name");
                        return Page();
                    }
                }
            }
            try
            {
                if (ModelState.IsValid && appUser != null) // turn into service
                {
                    this.SiteUser = new SiteUser { AppUser = appUser, Site = this.Site, IsAdmin = true };
                    _context.Sites.Add(Site);
                    _context.SiteUsers.Add(SiteUser);
                    await _context.SaveChangesAsync();
                }
            }
            catch
            {
                Message = "There was an issue saving the data as entered.";
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
