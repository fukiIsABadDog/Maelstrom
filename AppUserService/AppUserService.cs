using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Security.Principal;
using EF_Models;
using EF_Models.Models;
using Microsoft.AspNetCore.Http;
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
            var querySiteUsers = 
                from SiteUser in _context.SiteUsers
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

        public async Task<Site?> GetSiteForCurrentAdminSiteUser(IIdentity user, int? id)  
        {
                var querySiteUsers =
                    from SiteUser in _context.SiteUsers
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

        public async Task<Site?> GetSiteForCurrentSiteUser(IIdentity user, int? id)  
        {
            var querySiteUsers =
                from SiteUser in _context.SiteUsers
                join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                where AppUser.Email == user.Name
                where Sites.SiteID == id
                where SiteUser.Deleted == null
                select Sites;

            return await querySiteUsers.FirstOrDefaultAsync();
        }

        public async Task<ICollection<TestResult>?> GetCurrentUserSiteTestResults(IIdentity user, int? id)
        {
            var querySiteUsers =
                from SiteUser in _context.SiteUsers
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
            var querySiteUsers =
                from SiteUser in _context.SiteUsers
                join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                join SiteType in _context.SiteTypes on Sites.SiteType equals SiteType
                where AppUser.Email == user.Name
                where Sites.SiteID == id
                select SiteUser;

            return await querySiteUsers.FirstOrDefaultAsync();
        }

        public async Task<SiteUser?> FindSiteUserFromTestResult(IIdentity user, TestResult testResult)
        {
            var siteUser =

                from SiteUser in _context.SiteUsers
                join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                join TestResult in _context.TestResults on SiteUser equals TestResult.SiteUser
                where AppUser.Email == user.Name
                where TestResult == testResult
                select SiteUser;

            return await siteUser.FirstOrDefaultAsync();
        }

        public async Task<Site?> FindSiteFromTestResult(TestResult testResult)
        {
            var site =
               from TestResult in _context.TestResults
               join SiteUser in _context.SiteUsers on testResult.SiteUserID equals SiteUser.SiteUserID
               join Site in _context.Sites on SiteUser.SiteID equals Site.SiteID
               select Site;

            return await site.FirstOrDefaultAsync();
        }

        public async Task<SiteUser?> FindAdminSiteUser(IIdentity user, Site site)
        {
            var siteUser =
                from SiteUser in _context.SiteUsers
                join Site in _context.Sites on SiteUser.SiteID equals Site.SiteID
                join AppUser in _context.AppUsers on SiteUser.AppUser equals AppUser
                where AppUser.Email == user.Name
                where SiteUser.IsAdmin == true
                where Site == site 
                select SiteUser;

            return await siteUser.FirstOrDefaultAsync();
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

       public async Task AddNewTestResult(TestResult testResult)
        {
            _context.TestResults.Add(testResult);

            await _context.SaveChangesAsync();
        }

       public async Task<TestResult?> FindTestResult(int id)
        {
            var testResult = await _context.TestResults.Select(x => x).Where(x => x.TestResultID == id).FirstOrDefaultAsync();

            return testResult;
        }

        public async Task EditTestResult(TestResult testResult)
        {
            _context.Attach(testResult).State = EntityState.Modified;

            await _context.SaveChangesAsync();

        }
        public async Task<AppUser> FindAppUser(IIdentity user)
        {
            var appUser = await _context.AppUsers.Where(x => x.Email == user.Name).FirstAsync();

            return appUser;
        }

        public async Task<Dictionary<int,string>> GetAllSiteTypes()
        {
            var siteTypes = await _context.SiteTypes.ToDictionaryAsync(k => k.SiteTypeID, v => v.Name);

            return siteTypes;
        }

        public async Task<byte[]?> ConvertImageForDb(IFormFile upload)
        {
            using (var memoryStream = new MemoryStream())
            {
                await upload.CopyToAsync(memoryStream);
                if (memoryStream.Length < 4194304)
                {
                    return memoryStream.ToArray();
                }
                else
                {
                    return null;
                }
            }
        }

        public string? ConvertImageFromDb(byte[]? DbImageData)
        {
            if (DbImageData != null && DbImageData.Length > 1 == true)
            {
                var base64 = Convert.ToBase64String(DbImageData);
                var imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                return imgSrc;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task SaveSite(Site site)
        {
            _context.Sites.Add(site);

            await _context.SaveChangesAsync();
        }

        public async Task SaveSiteUser(SiteUser siteUser)
        {
           
            try
            {
                _context.SiteUsers.Add(siteUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw (new Exception(message:"There was an issue saving the data."));
            }

        }

        public bool SiteExists(int id)
        {
            return (_context.Sites?.Any(e => e.SiteID == id)).GetValueOrDefault();
        }

        public async Task EditSite(Site site)
        {
            _context.Attach(site).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SiteExists(site.SiteID))
                {
                    throw (new Exception("There was an issues saving the new data"));
                }
            }
        }

        public async Task<Site?> GetSite(int id)
        {
            var site = await _context.Sites.FirstOrDefaultAsync(e => e.SiteID == id);
            
            return site;
        }

        public async Task<AppUser?> GetAppUser(string email)
        {
            var appUser = await _context.AppUsers.FirstOrDefaultAsync(x => x.Email == email);

            return appUser;
        }



        public async Task<SiteUser?> GetSiteUser(int siteId, AppUser appUser)
        {

            var siteUserToBeRestored = await _context.SiteUsers.Where(x => x.SiteID == siteId)
                    .Where(x => x.AppUser == appUser)
                    .FirstOrDefaultAsync();

            return siteUserToBeRestored;
        }

        public async Task RestoreSiteUser(SiteUser siteUser)

        {
            siteUser.Deleted = null;
            _context.Attach(siteUser)
                .Property(p => p.Deleted).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                throw new Exception("There was an error saving this to the database");
            }
        }

        public async Task<SiteUser?> GetSiteUserIncludeAppUser(int siteUserId)
        {
            var siteUserWithAppUser = await _context.SiteUsers.Where(
                x => x.SiteUserID == siteUserId).Include(
                x => x.AppUser).FirstOrDefaultAsync();

            return siteUserWithAppUser;
        }

        public async Task<SiteUser?> GetSiteUser(int siteId, string appUserEmail)
        {
            var siteUser = await _context.SiteUsers.Where(
                x => x.AppUser.Email == appUserEmail)
                .Where(x => x.SiteID == siteId)
                .FirstOrDefaultAsync();

            return siteUser;
        }

        public async Task SoftDeleteSiteUser(int siteUserId)
        {
            var siteUserToBeSoftDeleted = 
                new SiteUser {
                    SiteUserID = siteUserId, 
                    Deleted = DateTime.Now 
            };

            _context.Attach(siteUserToBeSoftDeleted)
                .Property(p => p.Deleted).IsModified = true;

            await _context.SaveChangesAsync();
        }

        public async Task<Site?> GetSiteFromSiteUser(int? siteUserId)
        {
            var site = await _context.SiteUsers
                .Where(x => x.SiteUserID == siteUserId)
                .Include(x => x.Site)
                .Select(x => x.Site)
                .FirstOrDefaultAsync();

            return site;
        }

        public async Task<AppUser?>GetAppUser(int siteUserId)
        {
            var associatedAppUser = await _context.SiteUsers
                .Where(x => x.SiteUserID == siteUserId)
               .Include(x => x.AppUser)
               .Select(x => x.AppUser)
               .FirstOrDefaultAsync();

            return associatedAppUser;
        }

       public async Task<SiteUser?> GetSiteUser(int siteUserId)
        {
            var siteUser = await _context.SiteUsers
                .FirstOrDefaultAsync(x => 
                x.SiteUserID == siteUserId);

            return siteUser;
        }

        public async Task EditSiteUser(SiteUser siteUser)
        {
            _context.Attach(siteUser)
                .Property(p => p.IsAdmin).IsModified = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new Exception(
                    "There was an error saving this to the database");
            }
        }

        public async Task<ICollection<SiteUser>> GetSiteUsersIncludeAppUsers(int siteId)
        {
            var siteUsers = await _context.SiteUsers
                .Where(x => x.SiteID == siteId)
                .Where(x => x.Deleted.HasValue != true)
                .Include(x => x.AppUser)
                .ToListAsync();

            return siteUsers;
        }

    }
}
