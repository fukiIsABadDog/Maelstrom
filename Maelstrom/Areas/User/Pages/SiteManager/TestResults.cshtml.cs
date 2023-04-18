using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.User.Pages.SiteManager
{
    public class TestResultsModel : PageModel
    {

        private readonly EF_Models.MaelstromContext _context;
        private readonly IAppUserService _appUserService;

        public TestResultsModel(EF_Models.MaelstromContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;
        }
        public AppUser? AppUser { get; set; }
        public Site Site { get; set; } = default!;
        public string? SiteImage { get; private set; }
        public ICollection<TestResult>? TestResults { get; set; }

        //think about async... need to read docs. Does service need to be async as well?
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Sites == null)
            {
                return NotFound();
            }

            this.AppUser = _appUserService.FindAppUser(User.Identity);
            var site = _appUserService.GetAppUserSite(AppUser, id);
            var results = _appUserService.GetUserSiteTestResults(AppUser, id);

            if (site == null)
            {
                return NotFound();
            }
                
            else
            {
                this.Site = site;

                this.TestResults = results;

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
