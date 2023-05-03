using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


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
        public int SiteId { get; set; }
        public SiteUser Admin { get; set; } = null!;
        [BindProperty]
        public SiteUser NewSiteUser { get; set; } = null!;
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

            SiteId = id.Value;
            Admin = currentSiteUser;
            return Page();
        }

        /// <summary>
        /// In testing - 5/3
        /// 
        /// issue: 404 is blank
        /// issue: client side validation needs to be reconfigured
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            var email = NewSiteUser.AppUser.Email;
            var appUser =  _context.AppUsers.Where( x => x.Email == email).FirstOrDefault(); // only on not working
            if (appUser == null)
            { 
                return NotFound();
            }
            var isAdmin = NewSiteUser.IsAdmin;
            var siteID = SiteId;
            var siteUser = new SiteUser { SiteID = siteID, AppUser = appUser, IsAdmin = isAdmin };
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
