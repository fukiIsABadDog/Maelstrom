using EF_Models.Models;
using System.Security.Principal;

namespace Maelstrom.Services
{
    public interface IAppUserService
    {// rename methods for consistency and conciseness
        AppUser FindAppUser(IIdentity user);
        ICollection<Site> CurrentUserSites(AppUser user);
        Site SelectedSite(ICollection<Site> sites, Site? currentSite);
        ICollection<TestResult> SelectedSiteTestResults(Site site);
        string? GetSiteType(Site site);
        (ICollection<Site>, Dictionary<int, String>) CurrentUsersSitesAndTypes(AppUser user);
        Site? GetAppUserSite(AppUser user, int? id);
        ICollection<TestResult>? GetUserSiteTestResults(AppUser user, int? id);
        SiteUser? GetSiteUser(AppUser user, int? id);

        SiteUser? CheckTestResultUser(AppUser user, TestResult testResult);
    }
}
