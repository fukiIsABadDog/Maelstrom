using EF_Models.Models;
using System.Security.Principal;

namespace Maelstrom.Services
{
    public interface IAppUserService

    {    // all comments can be deleted here they are only for personal use!
         // 1) rename methods for consistency and conciseness
         // 2) organize methods according to primary callers


        Task<ICollection<Site>> GetCurrentUserSites(IIdentity user);
        Site GetSelectedSite(ICollection<Site> sites, Site? currentSite);
        Task<ICollection<TestResult>?> GetSelectedSiteTestResults(int id);
        Task<string?> GetSiteType(Site site);
        Task<Dictionary<int, string>> CreateSiteTypeDictionary();
        Task<Site?> GetCurrentUserSite(IIdentity user, int? id, bool isAdmin);
        Task<ICollection<TestResult>?> GetCurrentUserSiteTestResults(IIdentity user, int? id);
        Task<SiteUser?> FindSiteUserFromUserIdentityAndSiteID(IIdentity user, int? id);
        Task<SiteUser?> FindSiteUserFromTestResult(IIdentity user, TestResult testResult);

        Task<Site?> FindSiteFromTestResult(TestResult testResult); // currently being implimented 5/19 1:40
        Task<SiteUser?> FindAdminSiteUser(IIdentity user, Site site); //currently being refactored 5/19 1:40

        Task<TestResult?> FindTestResult(int? id);
        Task DeleteTestResult(int id);
        Task DeleteSite(int id);

        Task AddNewTestResult(TestResult testResult);
        Task<TestResult?> FindTestResult(int id);
        Task EditTestResult(TestResult testResult);

        Task<AppUser> FindAppUser(IIdentity user);
    }
}
