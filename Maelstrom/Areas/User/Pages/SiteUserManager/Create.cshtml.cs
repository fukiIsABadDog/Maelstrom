using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
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
        public Site Site { get; set; } = new Site();
        public SiteUser Admin { get; set; } = null!;
        [BindProperty]
        public SiteUser NewSiteUser { get; set; } = null!;
        
        [BindProperty]
       public bool IsAdmin { get; set; }

        [EmailAddress]
        [BindProperty]
        public string Email { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var currentUser = User.Identity!;
            var currentSiteUser = await _appUserService.GetSiteUser(currentUser, id);
            
            if (currentSiteUser == null || currentSiteUser.IsAdmin == false) 
            {
                return Forbid();          
            }

            Site = await _context.Sites.FirstAsync(x => x.SiteID == id)!;
            Admin = currentSiteUser;
            return Page();
        }

        /// <summary>
        /// Fully Functional
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {

            //BUG HERE!! Needs to check app user at THAT SITE! Not all sites in DB!
            var appUser =  _context.AppUsers.Where( x => x.Email == Email).FirstOrDefault(); 
            if (appUser == null)
            {
                Message = "That Email is not valid.";
                return Page();
            }

            var existingUser = _context.SiteUsers.FirstOrDefault(x => x.AppUser == appUser);
            if (existingUser != null) 
            {
                Message = "That User already has privileges assigned. Please visit Edit Page";
                return Page();
            }

            var siteUser = new SiteUser { Site = Site, AppUser = appUser, IsAdmin = IsAdmin };
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

            return RedirectToPage("/SiteManager/Index");
        }
    }
}
