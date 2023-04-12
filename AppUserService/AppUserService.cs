using Microsoft.AspNetCore.Identity;
using EF_Models.Models;
using EF_Models;
using System.Security.Principal;

namespace Maelstrom.Services
{
    public class AppUserService: IAppUserService
    {
        private readonly MaelstromContext _context;

       
        
        
        public AppUserService(MaelstromContext context)
        {

            _context = context;
        }
        
        // this might require middleware... still figuring this one out

        // The idea is to be able to reuse user code accross all user pages


        public AppUser FindAppUser(IIdentity user)
        {
            var currentAppUser = _context.Users.FirstOrDefault(x => x.Email == "Default@Maelstrom.com");

            if(user.IsAuthenticated != false) 
            {
                currentAppUser = _context.Users.FirstOrDefault(x => x.UserName == user.Name);

               
            }

            return currentAppUser;

        }

        public ICollection<Site> CurrentUserSites(AppUser user)
        {
            if(user.Email != "Default@Maelstrom.com")
            {
                var querySiteUsers = from SiteUser in _context.SiteUsers
                                     join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                     select new
                                     {
                                         site = SiteUser.Site,
                                         appUser = SiteUser.AppUser
                                     };
                var usersSites = querySiteUsers.Select(x => x).Where(x => x.appUser.Id == user.Id).Select(x => x.site).ToList();
                return usersSites;
            }

            else
            { // this may cause issues
                return new List<Site>();
            }
        }

       //public Site SelectedSite(ICollection<Site> sites);
       //public TestResult SelectedSiteTestResults(Site site);


    }
}