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

        //public DisplayModel myModel { get; set; }

        public ICollection<SiteType>? MySiteTypes { get; set; }
        public AppUser? CurrentAppUser { get; private set; }

        public IList<Site>? CurrentUserSites { get; private set; }


        public Dictionary<int, string>? myDictionary { get; set; } = new Dictionary<int, string> { };

    


        public void OnGet()
        {
            CurrentAppUser = _appUserService.FindAppUser(User.Identity);
            //CurrentUserSites = _appUserService.CurrentUserSites(CurrentAppUser).ToList();

            var querySiteUsers = from SiteUser in _context.SiteUsers
                                 join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                 join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                 join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                 where AppUser == CurrentAppUser
                                 select new
                                 {
                                     siteUser = SiteUser,
                                     sites = Sites,
                                     appUser = AppUser,
                                     siteType = SiteType

                                 };
            CurrentUserSites= querySiteUsers.Select(x => x.sites).ToList();

            MySiteTypes = querySiteUsers.Select(x => x.siteType).ToList();
            
            foreach(var site in MySiteTypes)
            {
                myDictionary.Add(site.SiteTypeID, site.Name);
            }

        }
    }

    //public class DisplayModel
    //{
    //    public IList<Site>? Sites { get; set; }
    //    public string SiteName { get; set; }
    //}
}
