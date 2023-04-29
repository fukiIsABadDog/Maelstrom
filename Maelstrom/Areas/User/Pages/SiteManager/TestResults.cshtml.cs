using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.SiteManager
{
    [Authorize]
    public class TestResultsModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;
        private readonly IAppUserService _appUserService;
        public TestResultsModel(EF_Models.MaelstromContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;
        }
        public IIdentity CurrentUser { get; set; } = null!;
        public Site Site { get; set; } = default!;
        public string? SiteImage { get; private set; }
        public ICollection<TestResult>? TestResults { get; set; }
        public string SiteTypeName { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Sites == null)
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
                var siteTypeName = await _appUserService.GetSiteType(Site);
                SiteTypeName = siteTypeName ?? " ";
                TestResults = await _appUserService.GetCurrentUserSiteTestResults(CurrentUser, id);
                //this 100% needs a service method
                if (Site.ImageData != null && Site.ImageData.Length > 1 == true)
                {
                    var base64 = Convert.ToBase64String(Site.ImageData);
                    var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                    SiteImage = imgSrc;
                }
            }
            return Page();
        }
    }
}
