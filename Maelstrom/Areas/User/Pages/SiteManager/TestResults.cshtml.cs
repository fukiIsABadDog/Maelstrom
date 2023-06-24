using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.SiteManager
{
    [Authorize]
    public class TestResultsModel : PageModel
    {
        private readonly IAppUserService _appUserService;
        public TestResultsModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }


        public IIdentity CurrentUser { get; set; } = null!;
        public Site Site { get; set; } = default!;
        public string? SiteImage { get; private set; }
        public ICollection<TestResult>? TestResults { get; set; }
        public string? SiteTypeName { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("That resource could not be located.");
            }

            CurrentUser = User.Identity!;
          
            var site = await _appUserService.GetSiteForCurrentSiteUser(CurrentUser, id); 
            if (site == null)
            {
                return Forbid();
            }
            else
            {
                Site = site;

                var siteTypeName = await _appUserService.GetSiteType(Site);
                SiteTypeName = siteTypeName ?? " ";

                var siteId = Site.SiteID;
                TestResults = await _appUserService.GetSelectedSiteTestResults(siteId);
                SiteImage = _appUserService.ConvertImageFromDb(Site.ImageData);
            }

            return Page();
        }
    }
}
