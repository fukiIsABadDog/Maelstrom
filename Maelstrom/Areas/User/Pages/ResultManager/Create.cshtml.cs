using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;

namespace Maelstrom.Areas.User.Pages.ResultManager
{

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

       
        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            this.AppUser = _appUserService.FindAppUser(User.Identity);
            var siteUser = _appUserService.GetSiteUser(AppUser, id);

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

            if (ModelState.IsValid)
            {
                var id = SiteID;
                TestResult.SiteUserID = SiteUserID;
                _context.TestResults.Add(TestResult);
                await _context.SaveChangesAsync();
                return RedirectToPage("/SiteManager/TestResults", new { id = SiteID.ToString() });
            }
            return Page();
        }
    }
}
