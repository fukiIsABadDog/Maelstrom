using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.ResultManager
{
    [Authorize]
    public class EditModel : PageModel
    {  

        private readonly IAppUserService _appUserService;
        public EditModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }


        [BindProperty]
        public TestResult TestResult { get; set; } = default!;
        [BindProperty]
        public SiteUser? SiteUser { get; set; }
        public IIdentity LoggedInUser { get; set; } = null!;


        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            LoggedInUser = User.Identity!;

            var testResult = await _appUserService.FindTestResult(id); 
            if (testResult == null)
            {
                return NotFound();
            }

            var siteUser = await _appUserService.FindSiteUserFromTestResult(LoggedInUser, testResult);
            if (siteUser != null)
            {
                SiteUser = siteUser;
                TestResult = testResult;

                return Page();
            }
            else
            {
                var site = await _appUserService.FindSiteFromTestResult(testResult);
                if (site == null) 
                {
                    return NotFound();
                }

                var adminSiteUser = await _appUserService.FindAdminSiteUser(LoggedInUser, site); 
                if (adminSiteUser == null)
                {
                    return Forbid();
                }

                SiteUser = adminSiteUser;
                TestResult = testResult;

                return Page();
            }  
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _appUserService.EditTestResult(TestResult);
          
            return RedirectToPage("/SiteManager/TestResults", new { id = SiteUser.SiteID.ToString() });
        }
    
    }
}
