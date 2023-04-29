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
        private readonly EF_Models.MaelstromContext _context;
        private readonly IAppUserService _appUserService;
        public DeleteModel(EF_Models.MaelstromContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;
        }

        [BindProperty]
        public TestResult TestResult { get; set; } = null!;
        [BindProperty]
        public SiteUser SiteUser { get; set; } = null!;
        public IIdentity CurrentUser { get; set; } = null!;

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return (NotFound());
            }
            CurrentUser = User.Identity!;
            var testResult = await _appUserService.FindTestResult(id);
            if (testResult == null)
            {
                return (NotFound());
            }
            var siteUser = await _appUserService.CheckAndReturnSiteUser(CurrentUser, testResult);

            if (siteUser == null || testResult == null)
            {
                return (NotFound());
            }
            else
            {
                SiteUser = siteUser!;
                TestResult = testResult!;
            }
            return (Page());
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _appUserService.DeleteTestResult(id);

            return RedirectToPage("/SiteManager/TestResults", new { id = SiteUser.SiteID.ToString() });

        }
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    var tr = new TestResult() { TestResultID = TestResult.TestResultID };
        //    _context.Remove(tr);
        //    await _context.SaveChangesAsync();

        //    return RedirectToPage("/SiteManager/TestResults", new { id = SiteUser.SiteID.ToString() });
        //}
    }
}

