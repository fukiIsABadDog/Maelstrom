using Microsoft.AspNetCore.Identity;
using EF_Models.Models;
using EF_Models;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom.Services
{
    /// <summary>
    /// This Service helps us reuse code accross multiple pages
    /// </summary>
    public class AppUserService : IAppUserService
    {
        private readonly MaelstromContext _context;
        public AppUserService(MaelstromContext context)
        {

            _context = context;
        }

        public AppUser FindAppUser(IIdentity user)
        {
            var currentAppUser = _context.Users.FirstOrDefault(x => x.Email == "Default@Maelstrom.com");

            if (user.IsAuthenticated != false)
            {
                currentAppUser = _context.Users.FirstOrDefault(x => x.UserName == user.Name);
            }
            return currentAppUser;
        }

        public ICollection<Site> CurrentUserSites(AppUser user)
        {
            if (user.Email != "Default@Maelstrom.com")
            {
                //var querySiteUsers = from SiteUser in _context.SiteUsers
                //                     join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                //                     select new
                //                     {
                //                         site = SiteUser.Site,
                //                         appUser = SiteUser.AppUser
                //                     };

                //var usersSites = querySiteUsers.Select(x => x).Where(x => x.appUser.Id == user.Id).Select(x => x.site).ToList();
                //return usersSites;


                var querySiteUsers = from SiteUser in _context.SiteUsers
                                     join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                     join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                     join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                     where AppUser == user
                                     select new
                                     {
                                         siteUser = SiteUser,
                                         sites = Sites,
                                         appUser = AppUser,
                                         siteType = SiteType

                                     };


                return querySiteUsers.Select(x => x.sites).ToList();

            }

            else
            { //probably could shorten this code

                var defaultSite = new Site { SiteID = 999, Name = "Default", Capacity = 0, Location = "Does not exist yet", SiteTypeID = 1 };
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
            if (currentSiteTestResultsQuery.Any()) { return currentSiteTestResultsQuery.ToList(); }
            else { return new List<TestResult>(); }
        }

        public string GetSiteType(Site site)
        {
            //This might get refactored into a enum or dictionary to reduce trips to the DB 

            var siteTypeQuery = _context.SiteTypes.Where(x => x.SiteTypeID == site.SiteTypeID).Select(x => x.Name);
            return siteTypeQuery.First();
        }

        public (ICollection<Site>, Dictionary<int, String>) CurrentUsersSitesAndTypes(AppUser user)
        {

            var querySiteUsers = from SiteUser in _context.SiteUsers
                                 join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                 join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                 join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                 where AppUser == user
                                 select new
                                 {
                                     siteUser = SiteUser,
                                     sites = Sites,
                                     appUser = AppUser,
                                     siteType = SiteType

                                 };
            //this might be making two calls to DB... revisit to optimize later
            var currentUserSites = querySiteUsers.Select(x => x.sites).ToList();

            var currentUserSiteTypes = querySiteUsers.Select(x => x.siteType).ToList();

            var currentUserSiteTypesDict = new Dictionary<int, String>();

            foreach (var site in currentUserSiteTypes)
            {
                currentUserSiteTypesDict.Add(site.SiteTypeID, site.Name);
            }

            return (currentUserSites, currentUserSiteTypesDict);
        }
    }
}
