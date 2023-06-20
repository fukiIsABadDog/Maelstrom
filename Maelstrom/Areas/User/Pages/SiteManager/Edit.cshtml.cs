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
    public class EditModel : PageModel
    {
        private readonly IAppUserService _appUserService;
        public EditModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [BindProperty]
        public Site Site { get; set; } = default!;
        [DisplayName("Upload New Image")]
        [BindProperty]
        [UploadFileExtensions(Extensions = ".jpeg,.jpg")]
        public IFormFile? Upload { get; set; }
        public string? ExistingSiteImage { get; private set; }
        public byte[]? NewSiteImage { get; set; }
        public IIdentity CurrentUser { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("That Resource could not be located.");
            }

            CurrentUser = User.Identity!;

            var site = await _appUserService.GetSiteForCurrentAdminSiteUser(CurrentUser, id);    
            
            var siteTypes = await _appUserService.GetAllSiteTypes();
            ViewData["SiteTypeID"] = new SelectList(siteTypes, "Key", "Value");

            if (site == null)
            {
                return Forbid();
            }
            else
            {
                Site = site;

                ExistingSiteImage = _appUserService.ConvertImageFromDb(Site.ImageData);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Upload != null)
            {
                var imageConvertedForDB = await _appUserService.ConvertImageForDb(Upload);
                NewSiteImage = imageConvertedForDB;

                if (NewSiteImage != null)
                {
                    Site.ImageData = NewSiteImage;
                }
                else
                {
                    ModelState.AddModelError("Upload", "That file is too large. It should be under 4MB.");

                    var siteTypes = await _appUserService.GetAllSiteTypes();
                    ViewData["SiteTypeID"] = new SelectList(siteTypes, "Key", "Value");

                    return Page();
                }
            }
            else
            {
                Site.ImageData = Site.ImageData;
            }

            await _appUserService.EditSite(Site);
          
            return RedirectToPage("./Index");
        }
    }
}


