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
        public IndexModel(IAppUserService appUserService, MaelstromContext context)
        {
            _context = context;
            _appUserService = appUserService;
        }


        public AppUser? CurrentAppUser { get; private set; }

        public IList<Site>? CurrentUserSites { get; private set; }

        

    


        public void OnGet()
        {
            CurrentAppUser = _appUserService.FindAppUser(User.Identity);
            CurrentUserSites = _appUserService.CurrentUserSites(CurrentAppUser).ToList();
           

        }
    }
}
