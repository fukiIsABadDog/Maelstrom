using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Maelstrom.Areas.User.Pages.SiteManager
{
    public class IndexModel : PageModel
    {
        private readonly IAppUserService _appUserService;
        public IndexModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }
        public AppUser? CurrentAppUser { get; private set; }
        public ICollection<Site>? CurrentUserSites { get; private set; }
        public void OnGet()
        {
            CurrentAppUser = _appUserService.FindAppUser(User.Identity);
            CurrentUserSites = _appUserService.CurrentUserSites(CurrentAppUser);

        }
    }
}
