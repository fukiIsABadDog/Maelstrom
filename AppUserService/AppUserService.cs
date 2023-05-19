using System.Security.Principal;
using EF_Models;
using EF_Models.Models;
using Microsoft.EntityFrameworkCore;
namespace Maelstrom.Services
{
    public class AppUserService : IAppUserService
    {
        private readonly MaelstromContext _context;
        public AppUserService(MaelstromContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Site>> GetCurrentUserSites(IIdentity user)
        {
            var querySiteUsers = from SiteUser in _context.SiteUsers
                                 join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                 join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                 join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                 where AppUser.Email == user.Name
                                 where Sites.Deleted == null
                                 where SiteUser.Deleted == null
                                 select Sites;
            return await querySiteUsers.Distinct().ToListAsync();
        }
        public Site GetSelectedSite(ICollection<Site> sites, Site? currentSite)
        {
            if (currentSite.Name == null)
            {
                return sites.First();
            }
            else
            {
                return sites.First(x => x.Name == currentSite.Name);
            }
        }
        public async Task<ICollection<TestResult>?> GetSelectedSiteTestResults(int Id)
        {
            var currentSiteTestResultsQuery = _context.TestResults.Select(x => x).Where(x => x.SiteUser.SiteID == Id).OrderByDescending(x => x.CreationDate);
            return await currentSiteTestResultsQuery.Where(t => t.Deleted == null).ToListAsync();
        }
        public async Task<string?> GetSiteType(Site site)
        {
            var siteTypeQuery = _context.SiteTypes.Where(x => x.SiteTypeID == site.SiteTypeID).Select(x => x.Name);
            return await siteTypeQuery.FirstOrDefaultAsync();
        }
        public async Task<Site?> GetCurrentUserSite(IIdentity user, int? id, bool onlyAdmin)
        {
            if (onlyAdmin == true)
            {
                var querySiteUsers = from SiteUser in _context.SiteUsers
                                     join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                     join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                     join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                     where AppUser.Email == user.Name
                                     where Sites.SiteID == id
                                     where SiteUser.Deleted == null
                                     where SiteUser.IsAdmin == true
                                     select Sites;
                return await querySiteUsers.FirstOrDefaultAsync();
            }
            else
            {
                var querySiteUsers = from SiteUser in _context.SiteUsers
                                     join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                     join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                     join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                     where AppUser.Email == user.Name
                                     where Sites.SiteID == id
                                     where SiteUser.Deleted == null
                                     select Sites;
                return await querySiteUsers.FirstOrDefaultAsync();
            }

        }

        public async Task<ICollection<TestResult>?> GetCurrentUserSiteTestResults(IIdentity user, int? id)
        {
            var querySiteUsers = from SiteUser in _context.SiteUsers
                                 join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                 join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                 join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                 join TestResult in _context.TestResults on SiteUser equals TestResult.SiteUser
                                 where AppUser.Email == user.Name
                                 where Sites.SiteID == id
                                 where TestResult.Deleted == null
                                 where Sites.Deleted == null
                                 select TestResult;
            return await querySiteUsers.OrderByDescending(x => x.CreationDate).ToListAsync();
        }
        public async Task<SiteUser?> FindSiteUserFromUserIdentityAndSiteID(IIdentity user, int? id)
        {
            var querySiteUsers = from SiteUser in _context.SiteUsers
                                 join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                                 join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                 join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                                 where AppUser.Email == user.Name
                                 where Sites.SiteID == id
                                 select SiteUser;
            return await querySiteUsers.FirstOrDefaultAsync();
        }
        public async Task<SiteUser?> CheckAndReturnSiteUser(IIdentity user, TestResult testResult)
        {
            var queryTrUser = from SiteUser in _context.SiteUsers
                              join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                              join TestResult in _context.TestResults on SiteUser equals TestResult.SiteUser
                              where AppUser.Email == user.Name
                              where TestResult == testResult
                              select SiteUser;
            return await queryTrUser.FirstOrDefaultAsync();
        }
        public async Task<SiteUser?> CheckAndReturnAdminSiteUser(IIdentity user, TestResult testResult)
        {
            var siteQuery = from TestResult in _context.TestResults
                            join Site in _context.Sites on TestResult.SiteUser.SiteID equals Site.SiteID
                            select Site;
            var site = await siteQuery.FirstOrDefaultAsync();

            var queryTrUser = from SiteUser in _context.SiteUsers
                              join Site in _context.Sites on SiteUser.SiteID equals Site.SiteID
                              join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                              where AppUser.Email == user.Name
                              where SiteUser.IsAdmin == true
                              where Site == site
                              select SiteUser;
            return await queryTrUser.FirstOrDefaultAsync();
        }
        public async Task<Dictionary<int, string>> CreateSiteTypeDictionary()
        {
            var siteTypesDict = new Dictionary<int, String>();
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
        public async Task DeleteSite(int id)
        {
            var site = new Site() { SiteID = id, Deleted = DateTime.Now };
            _context.Attach(site).Property(p => p.Deleted).IsModified = true;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTestResult(int id)
        {
            var testResult = new TestResult() { TestResultID = id, Deleted = DateTime.Now };
            _context.Attach(testResult).Property(p => p.Deleted).IsModified = true;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// returns siteuser that matches AppUser and Site if that SiteUser has not been soft Deleted
        /// </summary>
        /// <param name="site"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        /// 
        // This has not been implimented yet... the whole SiteUserManager area needs to be rafactored into services
        public async Task<SiteUser?> ValidateAndReturnSiteUser(Site site, IIdentity user)
        {
            var query = from SiteUser in _context.SiteUsers
                        join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                        join Site in _context.Sites on SiteUser.Site equals Site
                        where AppUser.Email == user.Name
                        where Site == site
                        where !SiteUser.Deleted.HasValue
                        select SiteUser;

            var siteUser = await query.FirstOrDefaultAsync();

            return siteUser;
        }

       public async Task AddNewTestResult(TestResult testResult)
        {
            _context.TestResults.Add(testResult);
            await _context.SaveChangesAsync();
        }
    }
}
