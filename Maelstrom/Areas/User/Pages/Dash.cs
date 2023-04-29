using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public AppUser CurrentAppUser { get; private set; }
        public ICollection<Site>? CurrentUserSites { get; private set; } = new List<Site>();
        [BindProperty(SupportsGet = true)]
        public Site CurrentSite { get; private set; }
        public string? SiteImage { get; private set; }

        public ICollection<TestResult>? CurrentSiteTestResults { get; private set; }

        //This will need addititional logic for user to save fish to his personal fish collection. As opposed to just the Site "owning" it.
        //public ICollection<Fish>? ThisUsersFish { get; private set; }

        //this doesn't need all these checks but I have to make sure nothing breaks for presentation
        public async Task<IActionResult> OnGetAsync(Site currentSite)
        {

            var currentAppUser = await _appUserService.FindAppUser(User.Identity);

            if (currentAppUser.Email == "Default@Maelstrom.com")
            {
                return NotFound();
            }
            CurrentAppUser = currentAppUser;

            var currentUserSites = await _appUserService.CurrentUserSites(CurrentAppUser);

            if (currentUserSites.Any() == false)
            {
                return RedirectToPage("/sitemanager/create");
            }
            else
            {
                CurrentUserSites = currentUserSites;
            }

            var selectedSite = _appUserService.SelectedSite(CurrentUserSites, currentSite);
            if (selectedSite == null)
            {
                return RedirectToPage("/sitemanager/create");
            }
            else
            {
                CurrentSite = selectedSite;
            }

            var testResults = await _appUserService.SelectedSiteTestResults(CurrentSite);
            if (testResults == null)
            {
                CurrentSiteTestResults = new List<TestResult>();
            }
            else
            {
                CurrentSiteTestResults = testResults;
            }

            var currentSiteType = await _appUserService.GetSiteType(CurrentSite);

            if (currentSiteType == null)
            {
                CurrentSiteType = "Unknown";
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
            else { }

            return Page();

        }
        public void OnPost()
        {

        }
    }
}
