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

        private readonly EF_Models.MaelstromContext _context;
        private readonly IAppUserService _appUserService;

        public EditModel(EF_Models.MaelstromContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;
        }
        [BindProperty]
        public TestResult TestResult { get; set; } = default!; //main object
        [BindProperty]
        public SiteUser? SiteUser { get; set; }// for validation and redirection
        public IIdentity CurrentUser { get; set; } = null!;

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            CurrentUser = User.Identity!;
            var testResult = await _context.TestResults.Select(x => x).Where(x => x.TestResultID == id).FirstOrDefaultAsync();
            if (testResult == null)
            {
                return Forbid();
            }
            var siteUser = await _appUserService.CheckAndReturnSiteUser(CurrentUser, testResult);

            if (siteUser == null || testResult == null)
            {
                //// this needs to be refactored in service
                //var testResultQuery = _context.TestResults.Select(x => x).Where(x => x.TestResultID == id);
                //var siteQuery =  testResultQuery.Include( x=> x.SiteUser.Site).Select( x=> x.SiteUser.Site);
                //var su = await siteQuery.Select(x => x.SiteUsers.Where( x => x.AppUser.Email == CurrentUser.Name)).SingleOrDefaultAsync();

                //if (su.First().IsAdmin != true)
                //{
                //    return (NotFound("You are not allowed to access this resource."));
                //}
                //SiteUser = su.First();

                var adminSiteUser = await _appUserService.CheckAndReturnAdminSiteUser(CurrentUser, testResult);
                if(adminSiteUser == null)
                {
                    return Forbid();
                }
                SiteUser = adminSiteUser;
                TestResult = testResult;

                return Page();



            }
           
            SiteUser = siteUser;
            TestResult = testResult;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TestResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(TestResult.TestResultID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/SiteManager/TestResults", new { id = SiteUser.SiteID.ToString() });
        }
        private bool SiteExists(int id)
        {
            return (_context.TestResults?.Any(e => e.TestResultID == id)).GetValueOrDefault();
        }
    }
}
