using EF_Models.Models;
using System.Security.Principal;

namespace Maelstrom.Services
{
    public interface IAppUserService
    {// rename methods for consistency and conciseness

        Task<ICollection<Site>> GetCurrentUserSites(IIdentity user);
        Site GetSelectedSite(ICollection<Site> sites, Site? currentSite);
        Task<ICollection<TestResult>?> GetSelectedSiteTestResults(int id);
        Task<string?> GetSiteType(Site site);
        Task<Dictionary<int, string>> CreateSiteTypeDictionary();
        Task<Site?> GetCurrentUserSite(IIdentity user, int? id, bool isAdmin);
        Task<ICollection<TestResult>?> GetCurrentUserSiteTestResults(IIdentity user, int? id);
        Task<SiteUser?> GetSiteUser(IIdentity user, int? id);
        Task<SiteUser?> CheckAndReturnSiteUser(IIdentity user, TestResult testResult);
        Task<SiteUser?> CheckAndReturnAdminSiteUser(IIdentity user, TestResult testResult);
        Task<TestResult?> FindTestResult(int? id);
        Task DeleteTestResult(int id);
        Task DeleteSite(int id);
        Task<SiteUser?> ValidateAndReturnSiteUser(Site site, IIdentity user);
    }
}
