using EF_Models;
using EF_Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace Maelstrom.Services
{
    /// <summary>
    /// This Service helps us reuse code accross multiple pages
    /// </summary>

    /// objective: Refactor for async -- doing now 4/26 -- stage 1 complete
    /// objective: Refactor for SRP -- doing now 4/26 
    /// objective: Need to test for nulls inside calling objects --doing now 4/26
    /// objective: see if you can get rid of some od these methods by making better db calls --doing now 4/26
    /// objective: If possible Simplify Queries
    /// objective:

    public class AppUserService : IAppUserService
    {
        private readonly MaelstromContext _context;
        public AppUserService(MaelstromContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> FindAppUser(IIdentity user)
        {
            // do this in a constuctor(line 26), we do not really need to have a full default modeled in the db.. it should be local.
            // until replace know that this may cause breaks in the code that depands on it --- so check for null references
            //var currentAppUser = await _context.Users.FirstOrDefault(x => x.Email == "Default@Maelstrom.com");

            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == user.Name);

        }
        public async Task<ICollection<Site>> CurrentUserSites(AppUser user)
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


            return await querySiteUsers.Select(x => x.sites).ToListAsync();



            //this should be done explictly in the contructor of the calling class -- see above

            //else
            //{
            //    var defaultSite = new Site { SiteID = 999, Name = "Default", Capacity = 0, Location = "Does not exist yet", SiteTypeID = 1 };
            //    var defaultSiteList = new List<Site>();
            //    defaultSiteList.Add(defaultSite);
            //    return defaultSiteList;
            //}
        }

        /// <summary>
        /// SelectedSite method its depending on something else... need to investigate
        /// </summary>
        /// <param name="sites"></param>
        /// <param name="currentSite"></param>
        /// <returns></returns>
        public async Task<Site?> SelectedSite(ICollection<Site> sites, Site currentSite)
        {

            // could use opperator for oneline expression or cleaned up some other way
            // also,  need to use ID attribute instead after view is modified

            //old code to be changed
            if (currentSite.Name != null)
            {
                return sites.First(x => x.Name == currentSite.Name);
            }
            else { return sites.FirstOrDefault(); }
        }
        public async Task<ICollection<TestResult>> SelectedSiteTestResults(Site site)
        {
            var currentSiteTestResultsQuery = _context.TestResults.Select(x => x).Where(x => x.SiteUser.SiteID == site.SiteID).OrderByDescending(x => x.CreationDate);
            if (currentSiteTestResultsQuery.Any())
            {
                return await currentSiteTestResultsQuery.ToListAsync();
            }
            else
            {
                return new List<TestResult>(); // this looks okay-- but to be consistant I will probably put this in the calling object
            }
        }

        /// <summary>
        /// This maybe be over complicationing the problem... I think we can reduce db calls with the extenstion method .Include()
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public async Task<string?> GetSiteType(Site site)
        {
            var siteTypeQuery = _context.SiteTypes.Where(x => x.SiteTypeID == site.SiteTypeID).Select(x => x.Name);
            return await siteTypeQuery.FirstOrDefaultAsync();
        }

        public async Task<Site?> GetAppUserSite(AppUser user, int? id)
        {

            //looks good but could probably be written cleaner
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

            return await querySiteUsers.Select(x => x.sites).FirstOrDefaultAsync();
        }

        public async Task<ICollection<TestResult>?> GetUserSiteTestResults(AppUser user, int? id)
        {
            //looks good but could probably be written cleaner

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

            return await querySiteUsers.Select(x => x.testResults).OrderByDescending(x => x.CreationDate).ToListAsync();
        }

        public async Task<SiteUser?> GetSiteUser(AppUser user, int? id)
        {
            //looks good but could probably be written cleaner

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

            return await querySiteUsers.Select(x => x.siteUser).FirstOrDefaultAsync();

        }

        public async Task<SiteUser?> CheckTestResultUser(AppUser user, TestResult testResult)
        {
            var queryTrUser = from SiteUser in _context.SiteUsers
                              join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                              join TestResult in _context.TestResults on SiteUser equals TestResult.SiteUser
                              where AppUser == user
                              where TestResult == testResult
                              select SiteUser;

            return await queryTrUser.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Again... I think I am approaching this problem the wrong way... this might get recoded completly or deleted
        /// </summary>

        public async Task<Dictionary<int, string>> GetAllSiteTypeValues()
        {
            var siteTypesDict = new Dictionary<int, String>(); // remove defaults and pass one in? or just use .include() some where else

            var siteList = await _context.SiteTypes.Select(x => x).ToListAsync();


            foreach (var site in siteList)
            {
                siteTypesDict.Add(site.SiteTypeID, site.Name);
            }

            return siteTypesDict;
        }

        public async Task<TestResult?> FindTestResult(int? id)
        {
            return await _context.TestResults.Select(x => x)
                .Where(x => x.TestResultID == id).FirstOrDefaultAsync();

        }
    }
}
