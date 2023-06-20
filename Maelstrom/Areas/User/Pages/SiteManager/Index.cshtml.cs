using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.Security.Policy;
using System.Security.Principal;

namespace Maelstrom.Areas.User.Pages.SiteManager
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAppUserService _appUserService;
        public IndexModel(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        public IIdentity CurrentUser { get; private set; } = null!;
        public IList<Site> CurrentUserSites { get; private set; } = null!;
        public Dictionary<int, string> SiteTypeDictionary { get; set; } = new Dictionary<int, string> { };
        public Dictionary<int, string?> ImageDictionary { get; set; } = new Dictionary<int, string?> { };
       
        
        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = User.Identity!;

            var sites = (IList<Site>)await _appUserService.GetCurrentUserSites(CurrentUser);
            var siteTypes = await _appUserService.CreateSiteTypeDictionary();

            CurrentUserSites = sites;

            if (siteTypes.Any())
            {
                SiteTypeDictionary = siteTypes;
            }

            foreach(var site in CurrentUserSites)
            {
                ImageDictionary.Add(site.SiteID, _appUserService.ConvertImageFromDb(site.ImageData));
            }

            return Page();
        }
    }
}
