using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.SiteUserManager
{
    [Authorize]
    public class CreateModel : PageModel
    {
        
        private readonly IAppUserService _appUserService;
        public CreateModel(IAppUserService appUserService )
        {
            _appUserService = appUserService;
        }

        [BindProperty]
        public string Message { get; set; }
        [BindProperty]
        public Site Site { get; set; }
        [BindProperty]
        public int SiteId { get; set; }
        public SiteUser Admin { get; set; } = null!;
        [BindProperty]
        public SiteUser NewSiteUser { get; set; } = null!;
        [BindProperty]
        public bool IsAdmin { get; set; }
        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public bool Restore { get; set; } = false;
        public SiteUser SiteUserToBeRestored { get; set; }

        public IIdentity CurrentUser = null!;


        public async Task<IActionResult> OnGetAsync(int? id, string? message, bool? restore)
        {
            if (id == null) { return BadRequest("That ID is not valid"); }

            if (message != null) { Message = message; }
         
            CurrentUser = User.Identity!;

            var currentSiteUser = await _appUserService.FindSiteUserFromUserIdentityAndSiteID(CurrentUser, id);
            if (currentSiteUser == null || currentSiteUser.IsAdmin == false) 
            {
                return Forbid();      
            }
            
            SiteId = id.Value;
            var site = await _appUserService.GetSite(SiteId);
            if (site == null) { return NotFound("That resource could not be located."); }

            Site = site;
            Admin = currentSiteUser;
           
            return Page();
        }

  
        public async Task<IActionResult> OnPostAsync()
        {
            SiteId = SiteId;

            var newAppUserForSite = await _context.AppUsers.Where( x => x.Email == Email).FirstOrDefaultAsync();
            if (newAppUserForSite == null)
            {
                Message = "That Email is not valid.";
                return await OnGetAsync(SiteId, Message, null); 
            }

            if (Restore == true) 
            {
                var siteUserToBeRestored = await _context.SiteUsers.Where( x=> x.SiteID == SiteId)
                    .Where(x => x.AppUser ==newAppUserForSite)
                    .FirstOrDefaultAsync();

                SiteUserToBeRestored = siteUserToBeRestored!; 

                SiteUserToBeRestored.Deleted = null;
                _context.Attach(SiteUserToBeRestored).Property(p => p.Deleted).IsModified = true;
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

            var existingUser = await _context.SiteUsers.Where(x => x.Site.SiteID == SiteId)
                .Where( x=> x.AppUser == newAppUserForSite).FirstOrDefaultAsync();
            if (existingUser != null) 
            {   
                if (existingUser.Deleted.HasValue)
                {
                    Message = "Would you like to restore that user?";
                    Restore = true;
                    return await OnGetAsync(SiteId, Message, Restore); ;
                }
                else
                {
                    Message = "That User already has privileges assigned.";
                    return await OnGetAsync(SiteId, Message, null);
                }
            }

            var site = _context.Sites.Find(SiteId);
            var siteUser = new SiteUser { Site = site, AppUser = newAppUserForSite, IsAdmin = IsAdmin };
            NewSiteUser = siteUser;

            try
            {
                _context.SiteUsers.Add(NewSiteUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        
            return RedirectToPage("./Index", new {id = SiteId});
        }

    }
}
