using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Maelstrom.Areas.User.Pages.ResultManager
{
    //notes:
    //to be fixed: user can enter empty results 

    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;
        private readonly IAppUserService _appUserService;
        public CreateModel(EF_Models.MaelstromContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;
        }


        [BindProperty]
        public TestResult TestResult { get; set; } = default!;

        [BindProperty]
        public int SiteUserID { get; set; }

        [BindProperty]
        public int SiteID { get; set; }
        public AppUser? AppUser { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            this.AppUser = await _appUserService.FindAppUser(User.Identity);
            var siteUser = await _appUserService.GetSiteUser(AppUser, id);

            var siteUserID = siteUser.SiteUserID;
            var siteID = siteUser.SiteID;

            if (siteUser == null)
            {
                return NotFound();
            }
            else
            {
                SiteID = siteID;
                SiteUserID = siteUserID;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            TestResult.CreationDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                TestResult.SiteUserID = SiteUserID;
                _context.TestResults.Add(TestResult);
                await _context.SaveChangesAsync();
                return RedirectToPage("/SiteManager/TestResults", new { id = SiteID.ToString() });
            }
            return Page();
        }
    }
}
