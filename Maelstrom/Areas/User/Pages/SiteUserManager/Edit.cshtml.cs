using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.SiteUserManager
{
    [Authorize]
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly IAppUserService _appUserService;
        public EditModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }


        public string Message { get; set; }
        public int SiteId { get; set; }
        public int SiteUserToBeEditedId { get; set; }
        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; }
        public  IIdentity CurrentUser { get; set; }
        public SiteUser SiteUserToBeEdited { get; set; }
        public AppUser AssociatedAppUser {get; set;} = new AppUser {FirstName = "Not Found"};


        public async Task<IActionResult> OnGetAsync(int? id, string? message) 
        {
            if (id == null) { return BadRequest("That ID is not valid"); }
            if (message != null) { Message = message; }

            CurrentUser = User.Identity!;

            var site = await _appUserService.GetSiteFromSiteUser(id);
            if (site == null)
            {
                return NotFound
                    ("We could not find anything matching that information.");
            }

            SiteId = site.SiteID;

            var currentSiteUser = await _appUserService.FindSiteUserFromUserIdentityAndSiteID(CurrentUser, SiteId);

            if (currentSiteUser == null || currentSiteUser.IsAdmin == false)
            {
                return Forbid();        
            }


            SiteUserToBeEditedId = id.Value;
            var associatedAppUser = await _context.SiteUsers.Where(x => x.SiteUserID == SiteUserToBeEditedId)
                .Include(x => x.AppUser).FirstOrDefaultAsync();
            if( associatedAppUser != null && associatedAppUser.AppUser != null){
                 AssociatedAppUser = associatedAppUser.AppUser;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            SiteId = SiteId;
            SiteUserToBeEditedId = SiteUserToBeEditedId;
            IsAdmin = IsAdmin;

            var siteUser = await _context.SiteUsers.FirstOrDefaultAsync(x => x.SiteUserID == SiteUserToBeEditedId);

            if (siteUser == null)
            {
                Message = "That User does not exist for this Site.";
                return await OnGetAsync(SiteId, Message);
            }
            if (siteUser.IsAdmin == true)
            {
                Message = "That User can not be modified, their privileges are set to Administrator.";
                return await OnGetAsync(SiteId, Message);
            }
            SiteUserToBeEdited = siteUser;
            SiteUserToBeEdited.IsAdmin = IsAdmin;

            _context.Attach(SiteUserToBeEdited).Property(p => p.IsAdmin).IsModified = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("There was an error saving this to the database");
            }

            return RedirectToPage("./Index", new { id = SiteId });
        }
    }
}


