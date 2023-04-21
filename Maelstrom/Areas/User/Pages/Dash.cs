using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Maelstrom.Areas.User.Pages
{

    public class DashModel : PageModel
    {
        private readonly IAppUserService _appUserService;
        public DashModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }
        public string CurrentSiteType { get; set; } = string.Empty;
        public AppUser? CurrentAppUser { get; private set; }
        public ICollection<Site>? CurrentUserSites { get; private set; }
        [BindProperty(SupportsGet = true)]
        public Site CurrentSite { get; private set; }
        public string? SiteImage { get; private set; }

        public ICollection<TestResult>? CurrentSiteTestResults { get; private set; }

        //This will need addititional logic for user to save fish to his personal fish collection. As opposed to just the Site "owning" it.
        //public ICollection<Fish>? ThisUsersFish { get; private set; }

        public void OnGet(Site currentSite)
        {
            //This block calls custom services to set this page's properities
            CurrentAppUser = _appUserService.FindAppUser(User.Identity);
            CurrentUserSites = _appUserService.CurrentUserSites(CurrentAppUser);
            CurrentSite = _appUserService.SelectedSite(CurrentUserSites, currentSite);
            CurrentSiteTestResults = _appUserService.SelectedSiteTestResults(CurrentSite);
            CurrentSiteType = _appUserService.GetSiteType(CurrentSite);





            if (CurrentSite.ImageData != null && CurrentSite.ImageData.Length > 1 == true)
            {
                var base64 = Convert.ToBase64String(CurrentSite.ImageData);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                SiteImage = imgSrc;
            }

        }
        public void OnPost()
        {

        }
    }
}
