using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.SiteUserManager
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly MaelstromContext _context;
        private readonly IAppUserService _appUserService;

        public DeleteModel(MaelstromContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;
        }
        [BindProperty]
        public string Message { get; set; }
        public IIdentity CurrentUser { get; set; }

        [BindProperty]
        public int SiteUserToBeDeletedId { get; set; }
        public SiteUser SiteUserToBeDeleted { get; set; }
        [BindProperty]
        public int SiteId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            SiteUserToBeDeletedId = id.Value;
            CurrentUser = User.Identity!;
            var currentSiteUser = await _context.SiteUsers.Where(x => x.AppUser.Email == CurrentUser.Name).FirstOrDefaultAsync();
            if (currentSiteUser == null || currentSiteUser.IsAdmin != true)
            {
                return NotFound();
            }
            var siteUserToBeDeleted = await _context.SiteUsers.Where(x => x.SiteUserID == SiteUserToBeDeletedId).FirstOrDefaultAsync();
            if (siteUserToBeDeleted == null || siteUserToBeDeleted.IsAdmin == true)
            {

                return NotFound();// this might need more work... but lets see if we can get away with it for now.

                // i am really leaning towards creating a custom 404 page with a string message url parameter
                //var site = await _context.Sites.FirstOrDefaultAsync(x => x.SiteID == SiteId);
                //Message = $"That User either does not exist or is currently a administrator of {site.Name}";

            }
            SiteId = siteUserToBeDeleted.SiteID;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var siteId = SiteId;
            var siteUserToBeDeletedId = SiteUserToBeDeletedId;

            var siteUserSoftDelete = new SiteUser { SiteUserID = siteUserToBeDeletedId, Deleted = DateTime.Now };
            _context.Attach(siteUserSoftDelete).Property(p => p.Deleted).IsModified = true;
            await _context.SaveChangesAsync();

            return RedirectToPage("./index", new {id = siteId});
        }
    }
}
