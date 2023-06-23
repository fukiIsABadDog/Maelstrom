using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
namespace Maelstrom.Areas.User.Pages.SiteUserManager
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IAppUserService _appUserService;
        public DeleteModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }


        [BindProperty]
        public string Message { get; set; }
        public IIdentity CurrentUser { get; set; }
        [BindProperty]
        public int SiteUserToBeDeletedId { get; set; }
        [BindProperty]
        public AppUser AppUserToBeRemoved { get; set; }
        [BindProperty]
        public int SiteId { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest(
                    "That Id was not valid. " +
                    "Use Browser Back button to return to previous page."
                    );
            }

            SiteUserToBeDeletedId = id.Value;
            var siteUserToBeDeleted = await _appUserService
                .GetSiteUserIncludeAppUser(SiteUserToBeDeletedId);

            if (siteUserToBeDeleted == null || siteUserToBeDeleted.IsAdmin == true)
            {
                return Forbid();
            }

            CurrentUser = User.Identity!;
            SiteId = siteUserToBeDeleted.SiteID;
            AppUserToBeRemoved = siteUserToBeDeleted.AppUser!;

            var currentSiteUser = await _appUserService
                .GetSiteUser(SiteId, CurrentUser.Name!);
    
            if(currentSiteUser == siteUserToBeDeleted)
            {
                return Page();
            }
            if (currentSiteUser == null || currentSiteUser.IsAdmin != true )
            {
                return Forbid();
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var siteId = SiteId;
            var siteUserToBeDeletedId = SiteUserToBeDeletedId;

            await _appUserService
                .SoftDeleteSiteUser(siteUserToBeDeletedId);
         
            return RedirectToPage("./index", new { id = siteId });
        }
    }
}
