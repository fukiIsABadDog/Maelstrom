using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.ResultManager
{
    [Authorize]
    public class CreateModel : PageModel
    {    
        private readonly IAppUserService _appUserService;
        public CreateModel( IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }


        [BindProperty]
        public TestResult TestResult { get; set; } = default!;
        [BindProperty]
        public int SiteUserID { get; set; }
        [BindProperty]
        public int SiteID { get; set; }
        public IIdentity LoggedInUser { get; set; } = null!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LoggedInUser = User.Identity!;

            var loggedInSiteUser = await _appUserService.FindSiteUserFromUserIdentityAndSiteID(LoggedInUser, id);

            if (loggedInSiteUser == null)
            {
                return NotFound();
            }
            else
            {
                SiteID = loggedInSiteUser.SiteID;
                SiteUserID = loggedInSiteUser.SiteUserID;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            TestResult.CreationDate = DateTime.Now;
            TestResult.SiteUserID = SiteUserID;

            if (ModelState.IsValid)
            {
                await _appUserService.AddNewTestResult(TestResult);
            
                return RedirectToPage("/SiteManager/TestResults", new { id = SiteID.ToString() });
            }

            return Page();
        }
    }
}
