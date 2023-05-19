using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.ResultManager
{
    [Authorize]
    public class DeleteModel : PageModel
    {  
        private readonly IAppUserService _appUserService;
        public DeleteModel( IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }


        [BindProperty]
        public TestResult TestResult { get; set; } = null!;
        [BindProperty]
        public SiteUser SiteUser { get; set; } = null!;
        public IIdentity LoggedInUser { get; set; } = null!;


        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return (NotFound());
            }

            LoggedInUser = User.Identity!;

            var testResult = await _appUserService.FindTestResult(id);
            if (testResult == null)
            {
                return (NotFound());
            }

            var siteUser = await _appUserService.FindSiteUserForTestResultFromUserIdentity(LoggedInUser, testResult);

            if (siteUser == null || testResult == null)
            {
                var adminSiteUser = await _appUserService.CheckAndReturnAdminSiteUser(LoggedInUser, testResult);

                if (adminSiteUser == null)
                {
                    return Forbid();
                }

                SiteUser = adminSiteUser;
                TestResult = testResult;

                return Page();
            }
            
            SiteUser = siteUser!;
            TestResult = testResult!;
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _appUserService.DeleteTestResult(id);

            return RedirectToPage("/SiteManager/TestResults", new { id = SiteUser.SiteID.ToString() });
        }
    }
}

