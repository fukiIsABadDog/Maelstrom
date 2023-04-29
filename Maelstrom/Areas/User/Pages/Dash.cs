using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.User.Pages
{
    [Authorize]
    public class DashModel : PageModel
    {
        private readonly IAppUserService _appUserService;
        //private readonly MaelstromContext _context;
        public DashModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
           
        }
        public string CurrentSiteType { get; set; } = string.Empty;
        //public AppUser CurrentAppUser { get; private set; }
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

            //var currentAppUser = await _appUserService.FindAppUser(User.Identity);

            //if (currentAppUser.Email == "Default@Maelstrom.com")
            //{
            //    return NotFound();
            //}
            //CurrentAppUser = currentAppUser;

            var currentUserSites = await _appUserService.CurrentUserSites(User.Identity);

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

            SiteImage = _appUserService.ConvertImage(CurrentSite.ImageData);
            return Page();

            // I think this way is slower for our use case -- but  I should test on the production server
            // This would be good if we wanted to reduce database traffic

            //this grabs everything at once
            //var querySiteUsers = from SiteUser in _context.SiteUsers
            //                     join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
            //                     join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
            //                     join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
            //                     join TestResult in _context.TestResults on SiteUser equals TestResult.SiteUser
            //                     where AppUser.Email == User.Identity.Name
            //                     where TestResult.Deleted == null
            //                     where Sites.Deleted == null
            //                     select new
            //                     {

            //                         sites = Sites,
            //                         siteType = SiteType,
            //                         testResults = TestResult

            //                     };
            //var listOfObjects = await querySiteUsers.ToListAsync();


            //// this selects users sites
            //var userSites = new List<Site>();
            //foreach (var obj in listOfObjects)
            //{
            //    if (!userSites.Contains(obj.sites))
            //    {
            //        userSites.Add(obj.sites);
            //    }
            //}
            //CurrentUserSites = userSites;

            //var selectedSite = _appUserService.SelectedSite(CurrentUserSites, currentSite);

            //if (selectedSite == null)
            //{
            //    return RedirectToPage("/sitemanager/create");
            //}
            //else
            //{
            //    CurrentSite = selectedSite;
            //}



            //var type = string.Empty;
            //foreach (var obj in listOfObjects)
            //{
            //    if (obj.sites == CurrentSite)
            //        type = obj.siteType.Name;
            //}

            //CurrentSiteType = type;

            //var testResults = new List<TestResult>();
            //foreach (var obj in listOfObjects)
            //{
            //    if (obj.sites == CurrentSite)
            //        testResults.Add(obj.testResults);
            //}

            //CurrentSiteTestResults = testResults;

            //SiteImage = _appUserService.ConvertImage(CurrentSite.ImageData);
            //return Page();


        }
        public void OnPost()
        {

        }
    }
}
