using EF_Models.Models;
using Maelstrom.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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
        public ICollection<SiteType>? MySiteTypes { get; set; }
        public IList<Site>? CurrentUserSites { get; private set; }
        public Dictionary<int, string>? SiteTypeDictionary { get; set; } = new Dictionary<int, string> { };

        public Dictionary<int, string?> ImageDictionary { get; set; } = new Dictionary<int, string?> { };
        public void OnGet()
        {
            CurrentAppUser = _appUserService.FindAppUser(User.Identity);
            var cusat = _appUserService.CurrentUsersSitesAndTypes(CurrentAppUser);
            CurrentUserSites = cusat.Item1.ToList();
            SiteTypeDictionary = cusat.Item2;

            foreach(var site in CurrentUserSites)
            {
                ImageDictionary.Add(site.SiteID, ImageConverter(site.ImageData));
            }

            var test = ImageDictionary.Values.ToList();
        }

        //turn this into service later
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
