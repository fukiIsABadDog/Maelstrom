using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.SiteManager
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
        public Site Site { get; set; } = default!;
        public SiteUser SiteUser { get; set; }
        public string? SiteImage { get; private set; }

        public IIdentity CurrentUser { get; set; } = null!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            CurrentUser = User.Identity!;
            var site = await _appUserService.GetCurrentUserSite(CurrentUser, id);

            if (site == null)
            {
                return NotFound();
            }
            else
            {
                Site = site;
                if (Site.ImageData != null && Site.ImageData.Length > 1 == true)
                {
                    var base64 = Convert.ToBase64String(Site.ImageData);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    SiteImage = imgSrc;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _appUserService.DeleteSite(id);

            return RedirectToPage("./Index");
        }
    }
}
