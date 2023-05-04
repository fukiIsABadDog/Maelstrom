using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Maelstrom.Areas.User.Pages.SiteUserManager
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly MaelstromContext _context;
        private readonly IAppUserService _appUserService;

        public CreateModel(MaelstromContext context, IAppUserService appUserService )
        {
            _context = context;
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

        public async Task<IActionResult> OnGetAsync(int? id, string? message, bool? restore)
        {

            if (id == null)
            {
                return BadRequest("That ID is not valid");
            }
            if (message != null) 
            {
                Message = message;
            }
         
            var currentUser = User.Identity!;
            var currentSiteUser = await _appUserService.GetSiteUser(currentUser, id);
            
            if (currentSiteUser == null || currentSiteUser.IsAdmin == false) 
            {
                return Forbid();// revisit          
            }
            
            SiteId = id.Value;
            var site = await _context.Sites.FirstAsync(x => x.SiteID == id)!;
            Site = Site;
            Admin = currentSiteUser;
           
            return Page();
        }

        /// <summary>
        /// Issues found
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            SiteId = SiteId;

            
            var appUser = await _context.AppUsers.Where( x => x.Email == Email).FirstOrDefaultAsync();

            if (appUser == null)
            {
                Message = "That Email is not valid.";
                return await OnGetAsync(SiteId, Message, null); 
            }

            if (Restore == true) // this edits the SiteUser instead of creating new one
            {
                var siteUserToBeRestored = await _context.SiteUsers.Where( x=> x.SiteID == SiteId)
                    .Where(x => x.AppUser ==appUser)
                    .FirstOrDefaultAsync();

                SiteUserToBeRestored = siteUserToBeRestored!; // already checked first go around

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
                .Where( x=> x.AppUser == appUser).FirstOrDefaultAsync();
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
            var siteUser = new SiteUser { Site = site, AppUser = appUser, IsAdmin = IsAdmin };
            NewSiteUser = siteUser;
            try
            {
                _context.SiteUsers.Add(NewSiteUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToPage("./Index", new {id = SiteId});
        }

    }
}
