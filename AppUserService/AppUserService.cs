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
            { //probably could shorten this code

                var defaultSite = new Site { SiteID = 999, Name = "Default", Capacity = 0, Location = "Does not exist yet" };
                var defaultSiteList = new List<Site>();
                defaultSiteList.Add(defaultSite);
                return defaultSiteList;
            }
        }

       public Site SelectedSite(ICollection<Site> sites, Site currentSite)
        {

            // could use opperator for oneline expression or cleaned up some other way
            // also,  need to use ID attribute instead after view is modified

                if (currentSite.Name != null)
                {
                    return sites.First(x => x.Name == currentSite.Name);
                }
                else { return sites.FirstOrDefault(); }
        }

        public ICollection<TestResult> SelectedSiteTestResults(Site site)
        {
            var currentSiteTestResultsQuery = _context.TestResults.Select(x => x).Where(x => x.SiteUser.SiteID == site.SiteID);
            if(currentSiteTestResultsQuery.Any()) { return  currentSiteTestResultsQuery.ToList(); }
            else { return new List<TestResult>(); }

           
        }


    }
}