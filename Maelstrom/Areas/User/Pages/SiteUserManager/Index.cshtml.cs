using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EF_Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.User.Pages.SiteUserManager
{
    [Authorize]

    public class IndexModel : PageModel
    {
        private readonly IAppUserService _appUserService;
        public IndexModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }


        public int SiteID { get; set; }
        public List<SiteUser> SiteUsers { get; set;} = new List<SiteUser>();
      


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest("That ID was not valid"); 
            }

            SiteID = id.Value;
            var siteUsers = await _appUserService.GetSiteUsersIncludeAppUsers(SiteID);
            SiteUsers = siteUsers.ToList();

            return Page();
        }
    }
}
