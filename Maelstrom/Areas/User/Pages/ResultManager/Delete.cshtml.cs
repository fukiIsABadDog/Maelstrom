using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public SiteUser SiteUser { get; set; }
        public AppUser AppUser { get; set; }
        // If I have time...I will research making a cookie for all this validation stuff.
        // I beleive it will improve performance... I really don't like going to the DB this much
        // Also reconfigure onget to be async.. this might require async service methods...needs research too
        public Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return Task.FromResult<IActionResult>(NotFound());
            }
            this.AppUser = _appUserService.FindAppUser(User.Identity);
            var testResult = _context.TestResults.Select(x => x).Where(x => x.TestResultID == id).FirstOrDefault();
            if (testResult == null)
            {
                return Task.FromResult<IActionResult>(NotFound());
            }
            var siteUser = _appUserService.CheckTestResultUser(AppUser, testResult);
            // need to impliment custom 404 page
            if (siteUser == null || testResult == null)
            {
                return Task.FromResult<IActionResult>(NotFound());
            }
            else
            {
                SiteUser = siteUser;
                TestResult = testResult;
            }
            return Task.FromResult<IActionResult>(Page());
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (TestResult == null || _context.TestResults == null)
            {
                return NotFound();
            }
            var tr = await _context.TestResults.FindAsync(TestResult.TestResultID);
            if (tr != null)
            {
                TestResult = tr;
                _context.TestResults.Remove(TestResult);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/SiteManager/TestResults", new { id = SiteUser.SiteID.ToString() });
        }
    }
}
