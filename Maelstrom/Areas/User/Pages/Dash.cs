using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;
namespace Maelstrom.Areas.User.Pages
{
    [Authorize]
    public class DashModel : PageModel
    {
        private readonly IAppUserService _appUserService;
        public DashModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }
        public string CurrentSiteType { get; set; } = string.Empty;
        public IIdentity CurrentUser { get; private set; }
        public ICollection<Site>? CurrentUserSites { get; private set; }
        [BindProperty(SupportsGet = true)]
        public Site CurrentSite { get; private set; } = null!;
        public string? SiteImage { get; private set; }
        public ICollection<TestResult>? CurrentSiteTestResults { get; private set; }
        //This will need addititional logic for user to save fish to his personal fish collection. As opposed to just the Site "owning" it.
        //public ICollection<Fish>? ThisUsersFish { get; private set; }
        public async Task<IActionResult> OnGetAsync(Site currentSite)
        {
            CurrentUser = User.Identity!;
            var currentUserSites = await _appUserService.GetCurrentUserSites(CurrentUser);
            if (currentUserSites.Any() == false)
            {
                return RedirectToPage("/sitemanager/create");
            }
            else
            {
                CurrentUserSites = currentUserSites;
            }
            CurrentSite = _appUserService.GetSelectedSite(CurrentUserSites, currentSite)!;
            var currentSiteID = CurrentSite.SiteID;
            var testResults = await _appUserService.GetSelectedSiteTestResults(currentSiteID);
            CurrentSiteTestResults = testResults;
            var currentSiteType = await _appUserService.GetSiteType(CurrentSite);
            if (currentSiteType == null)
            {
                CurrentSiteType = "N/A";
            }
            else
            {
                CurrentSiteType = currentSiteType;
            }
            if (CurrentSite.ImageData != null && CurrentSite.ImageData.Length > 1 == true)
            {
                var base64 = Convert.ToBase64String(CurrentSite.ImageData);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                SiteImage = imgSrc;
            }

            return Page();
        }
        public void OnPost()
        {
        }
    }
}
