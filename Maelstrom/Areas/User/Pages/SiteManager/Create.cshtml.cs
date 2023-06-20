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
       private readonly IAppUserService _appUserService;
        public CreateModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }


        public SelectList? SiteTypes { get; set; }
        public IIdentity LoggedInUser { get; set; } = null!;
        [BindProperty]
        public Site Site { get; set; } = default!;
        public SiteUser SiteUser { get; set; } = null!;
        [DisplayName("Upload Image")]
        [UploadFileExtensions(Extensions = ".jpeg,.jpg")]
        public IFormFile? Upload { get; set; }
        [BindProperty]
        public AppUser AppUser { get; set; } = null!;
        public string? Message { get; set; }
        public Byte[]? ConvertedImage { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            
            var siteTypes = await _appUserService.GetAllSiteTypes();
            ViewData["SiteTypeID"] = new SelectList(siteTypes, "Key", "Value");

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            LoggedInUser = User.Identity!;
            var appUser = await _appUserService.FindAppUser(LoggedInUser);

            if (Upload != null)
            {
                var convertedImage = await _appUserService.ConvertImageForDb(Upload);
                ConvertedImage = convertedImage;
                if (ConvertedImage != null)
                {
                    this.Site.ImageData = ConvertedImage;      
                }
                else
                {
                    ModelState.AddModelError("Upload", "That file is too large. It should be under 4MB.");

                    var siteTypes = await _appUserService.GetAllSiteTypes();
                    ViewData["SiteTypeID"] = new SelectList(siteTypes, "Key", "Value");

                    return Page();
                }
            }
            else {} 

            SiteUser = new SiteUser { AppUser = appUser, Site = this.Site, IsAdmin = true };
            await _appUserService.SaveSite(Site);
            await _appUserService.SaveSiteUser(SiteUser);
                
            return RedirectToPage("./Index");
        }
    }
}
