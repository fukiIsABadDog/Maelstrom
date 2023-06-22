using EF_Models.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;
using System.Web.Mvc;

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
        Task<Site?> GetSiteForCurrentAdminSiteUser(IIdentity user, int? id);
        Task<Site?> GetSiteForCurrentSiteUser(IIdentity user, int? id);
        Task<ICollection<TestResult>?> GetCurrentUserSiteTestResults(IIdentity user, int? id);
        Task<SiteUser?> FindSiteUserFromUserIdentityAndSiteID(IIdentity user, int? id);
        Task<SiteUser?> FindSiteUserFromTestResult(IIdentity user, TestResult testResult);

        Task<Site?> FindSiteFromTestResult(TestResult testResult); 
        Task<SiteUser?> FindAdminSiteUser(IIdentity user, Site site); 

        Task<TestResult?> FindTestResult(int? id);
        Task DeleteTestResult(int id);
        Task DeleteSite(int id);

        Task AddNewTestResult(TestResult testResult);
        Task<TestResult?> FindTestResult(int id);
        Task EditTestResult(TestResult testResult);

        Task<AppUser> FindAppUser(IIdentity user);
        Task<Dictionary<int, string>> GetAllSiteTypes();
        Task<byte[]?> ConvertImageForDb(IFormFile upload);
        string? ConvertImageFromDb(byte[]? DbImageData);
        Task SaveSite(Site site);
        Task SaveSiteUser(SiteUser siteUser);

        Task EditSite(Site site);
        Boolean SiteExists(int id);

        Task<Site?> GetSite(int id);
        Task<AppUser?> GetAppUser(string email);
        Task<SiteUser?> GetSiteUserToBeRestored(int siteId, AppUser appUser);
    }
}
