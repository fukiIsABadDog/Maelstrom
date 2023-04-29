using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public AppUser CurrentAppUser { get; private set; } = new AppUser() { FirstName = "default", Email = "Default@Maelstrom.com" };
        public ICollection<SiteType>? MySiteTypes { get; set; }
        public IList<Site> CurrentUserSites { get; private set; } = null!;
        public Dictionary<int, string> SiteTypeDictionary { get; set; } = new Dictionary<int, string> { };
        public Dictionary<int, string?> ImageDictionary { get; set; } = new Dictionary<int, string?> { };
        public async Task<IActionResult> OnGetAsync()
        {
            var user = User.Identity!;
            var sites = (IList<Site>)await _appUserService.GetCurrentUserSites(user);
            var siteTypes = await _appUserService.CreateSiteTypeDictionary();
            CurrentUserSites = sites;
            if (siteTypes.Any())
            {
                SiteTypeDictionary = siteTypes;
            }
            foreach (var site in CurrentUserSites)
            {
                ImageDictionary.Add(site.SiteID, ImageConverter(site.ImageData));
            }
            return Page();
        }

        public string ImageConverter(byte[]? dbImage)
        {
            if (dbImage != null && dbImage.Length > 1 == true)
            {
                var base64 = Convert.ToBase64String(dbImage);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                return imgSrc;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
