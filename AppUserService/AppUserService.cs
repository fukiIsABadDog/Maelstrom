using EF_Models;
using EF_Models.Models;
using System.Security.Principal;

namespace Maelstrom.Services
{
    /// <summary>
    /// This Service helps us reuse code accross multiple pages
    /// </summary>

    /// Note: Refactor & think about Async
    /// Note: Dictionary? Need to reduce DB calls
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
            {
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
            var currentSiteTestResultsQuery = _context.TestResults.Select(x => x).Where(x => x.SiteUser.SiteID == site.SiteID).OrderByDescending(x => x.CreationDate);
            if (currentSiteTestResultsQuery.Any()) { return currentSiteTestResultsQuery.ToList(); }
            else { return new List<TestResult>(); }
        }

        public string? GetSiteType(Site site)
        {
            //This might get refactored into a enum or dictionary to reduce trips to the DB 

            var siteTypeQuery = _context.SiteTypes.Where(x => x.SiteTypeID == site.SiteTypeID).Select(x => x.Name);
            return siteTypeQuery.FirstOrDefault();
        }

        public Site? GetAppUserSite(AppUser user, int? id)
        {
            var querySiteUsers = from SiteUser in _context.SiteUsers
                                 join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                 join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                 join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                 where AppUser == user
                                 where Sites.SiteID == id
                                 select new
                                 {
                                     siteUser = SiteUser,
                                     sites = Sites,
                                     appUser = AppUser,
                                     siteType = SiteType

                                 };

            var siteUser = querySiteUsers.Select(x => x.siteUser.SiteUserID).FirstOrDefault();

            return querySiteUsers.Select(x => x.sites).FirstOrDefault();

        }

        public ICollection<TestResult>? GetUserSiteTestResults(AppUser user, int? id)
        {
            var querySiteUsers = from SiteUser in _context.SiteUsers
                                 join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                 join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                 join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                 join TestResult in _context.TestResults on SiteUser equals TestResult.SiteUser
                                 where AppUser == user
                                 where Sites.SiteID == id
                                 select new
                                 {
                                     siteUser = SiteUser,
                                     sites = Sites,
                                     appUser = AppUser,
                                     siteType = SiteType,
                                     testResults = TestResult

                                 };


            var results = querySiteUsers.Select(x => x.testResults).OrderByDescending(x => x.CreationDate).ToList();
            return (results);
        }

        public SiteUser? GetSiteUser(AppUser user, int? id)
        {
            var querySiteUsers = from SiteUser in _context.SiteUsers
                                 join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                 join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                 join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                 where AppUser == user
                                 where Sites.SiteID == id
                                 select new
                                 {
                                     siteUser = SiteUser,
                                     sites = Sites,
                                     appUser = AppUser,
                                     siteType = SiteType

                                 };

            return querySiteUsers.Select(x => x.siteUser).FirstOrDefault();

        }

        public SiteUser? CheckTestResultUser(AppUser user, TestResult testResult)
        {
            var queryTrUser = from SiteUser in _context.SiteUsers
                              join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                              join TestResult in _context.TestResults on SiteUser equals TestResult.SiteUser
                              where AppUser == user
                              where TestResult == testResult
                              select SiteUser;

            return queryTrUser.FirstOrDefault();
        }

        public Dictionary<int, string> GetAllSiteTypeValues()
        {
            var siteTypesDict = new Dictionary<int, String>();

            var siteList = _context.SiteTypes.Select(x => x).ToList();


            foreach (var site in siteList)
            {
                siteTypesDict.Add(site.SiteTypeID, site.Name);
            }

            return siteTypesDict;
        }

    }
}
