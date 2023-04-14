using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Areas.User.Pages.SiteManager
{
    public class IndexModel : PageModel
    {
        private readonly EF_Models.MaelstromContext _context;
        private readonly IAppUserService _appUserService;
        public IndexModel(IAppUserService appUserService,MaelstromContext context )
        {
            _context = context;
            _appUserService = appUserService;
        }

        
        public AppUser? CurrentAppUser { get; private set; }
        public ICollection<Site>? CurrentUserSites { get; private set; }
        public ICollection<SiteType> AllSiteTypes { get; private set; }
 
        public IList<Site> Site { get; set; } = default!;

        //public async Task OnGetAsync()
        //{
        //    if (_context.Sites != null)
        //    {
        //       // cant figure this out right now
        //    }
        //}


        public void OnGet()
        {
            CurrentAppUser = _appUserService.FindAppUser(User.Identity);
            CurrentUserSites = _appUserService.CurrentUserSites(CurrentAppUser);

        }
    }
}
