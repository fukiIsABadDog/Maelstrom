using EF_Models.Models;
using System.Security.Principal;

namespace Maelstrom.Services
{
    public interface IAppUserService
    {// rename methods for consistency and conciseness
        Task<AppUser?> FindAppUser(IIdentity user);
        Task<ICollection<Site>> CurrentUserSites(IIdentity user);
        Site? SelectedSite(ICollection<Site> sites, Site? currentSite);
        Task<ICollection<TestResult>?> SelectedSiteTestResults(Site site);
        Task<string?> GetSiteType(Site site);
        Task<Dictionary<int, string>> GetAllSiteTypeValues();
        Task<Site?> GetAppUserSite(AppUser user, int? id);
        Task<ICollection<TestResult>?> GetUserSiteTestResults(AppUser user, int? id);
        Task<SiteUser?> GetSiteUser(AppUser user, int? id);
        Task<SiteUser?> CheckTestResultUser(AppUser user, TestResult testResult);
        Task<TestResult?> FindTestResult(int? id);
        Task DeleteTestResultAsync(int id);
        Task DeleteSiteAsync(int id);
    }
}
