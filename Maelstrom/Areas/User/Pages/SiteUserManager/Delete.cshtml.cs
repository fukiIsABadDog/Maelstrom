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
                return BadRequest("That Id was not valid. Use Browser Back button to return to previous page.");
            }
            SiteUserToBeDeletedId = id.Value;
            CurrentUser = User.Identity!;
            var currentSiteUser = await _context.SiteUsers.Where(x => x.AppUser.Email == CurrentUser.Name).FirstOrDefaultAsync();
            if (currentSiteUser == null || currentSiteUser.IsAdmin != true)
            {
                return Forbid();
            }
            var siteUserToBeDeleted = await _context.SiteUsers.Where(x => x.SiteUserID == SiteUserToBeDeletedId).FirstOrDefaultAsync();
            if (siteUserToBeDeleted == null || siteUserToBeDeleted.IsAdmin == true)
            {              
                return Forbid();        
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
