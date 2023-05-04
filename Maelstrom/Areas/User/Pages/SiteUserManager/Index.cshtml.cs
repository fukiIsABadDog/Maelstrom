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
        private readonly MaelstromContext _context;
        private readonly IAppUserService _appUserService;
        public IndexModel(MaelstromContext context, IAppUserService appUserService)
        {
            _context = context;
            _appUserService = appUserService;
        }
        public int SiteID { get; set; }
        public List<SiteUser> SiteUsers { get; set;}
        [BindProperty]
        public string Message { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return BadRequest("That ID was not valid"); 
            }

            SiteID = id.Value;
            var siteUsers = await _context.SiteUsers.Where(x =>  x.SiteID == SiteID).Where(x => x.Deleted.HasValue != true).Include(x => x.AppUser).Include( x => x.Site).ToListAsync();
            SiteUsers = siteUsers;

            return Page();
        }
    }
}
