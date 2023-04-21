using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Maelstrom.Areas.User.Pages.ResultManager
{

    // not complete! developing now 4/21
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
        public TestResult TestResult { get; set; }
        [BindProperty]
        public SiteUser SiteUser { get; set; }
        [BindProperty]
        public int SiteID { get; set; }
        public AppUser AppUser { get; set; }


        // If I have time. I will think about making a cookie for all this validation stuff.
        // I beleive it will improve performance... I really don't like going to the DB this much
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            this.AppUser = _appUserService.FindAppUser(User.Identity);
            var testResult = _context.TestResults.Select(x => x).Where(x => x.TestResultID == id).FirstOrDefault();
            if (testResult == null)
            {
                return NotFound();
            }
            var siteUser = _appUserService.CheckTestResultUser(AppUser, testResult);

            // need to impliment custom 404 page
            if (siteUser == null || testResult == null)
            {
                return NotFound();
            }
            else
            {
                SiteUser = siteUser;
                TestResult = testResult;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Sites == null)
            {
                return NotFound();
            }
            var tr = await _context.TestResults.FindAsync(id);

            if (tr != null)
            {
                _context.TestResults.Remove(tr);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/SiteManager/TestResults", new { id = SiteID.ToString() });

        }
    }
}
