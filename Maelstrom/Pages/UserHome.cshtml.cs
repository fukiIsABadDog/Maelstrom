using EF_Models.Models;
using Maelstrom.Controllers;
using Maelstrom.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace Maelstrom
{
 
    public class UserHomeModel : PageModel
    {
        private readonly MaelstromContext _context;
        private readonly IAppUserService _appUserService;
        public UserHomeModel(MaelstromContext context, IAppUserService appUserService)
        {

            _context = context;
            _appUserService = appUserService;
            
        }

        public string Message  {get; set;} = string.Empty;
        public string CurrentSiteType { get; private set; } = string.Empty;
        public AppUser? CurrentAppUser { get; private set; }

        public ICollection<Site>? CurrentUserSites { get; private set; }


        [BindProperty(SupportsGet = true)]
        public Site CurrentSite { get; private set; } = new Site { SiteID = 999, Name= "Default", Capacity = 0, Location = "Does not exist yet"}; 

        //extention types will change
        public string SiteImage { get; set; }

        public ICollection<TestResult>? CurrentSiteTestResults  { get; private set; } 

        //This will need addititional logic for user to save fish to his personal fish collection. As opposed to just the Site "owning" it.
        //public ICollection<Fish>? ThisUsersFish { get; private set; }

        public void OnGet(Site currentSite)
        {

            CurrentAppUser = _appUserService.FindAppUser(User.Identity); /// This Works!!!!!!
         

            CurrentUserSites = _appUserService.CurrentUserSites(CurrentAppUser);


                    if(currentSite.Name != null)
                    {
                        // needs model validation
                       CurrentSite = CurrentUserSites.First(x => x.Name == currentSite.Name);

                    }
                    else
                    {
                        var firstCurrentSite = CurrentUserSites.FirstOrDefault();
                        if (firstCurrentSite != null)
                        {
                            CurrentSite = firstCurrentSite;

                        }
                    }

                    try
                    {
                        var currentSiteTestResultsQuery = _context.TestResults.Select(x => x).Where(x => x.SiteUser.SiteID == CurrentSite.SiteID);
                        CurrentSiteTestResults = currentSiteTestResultsQuery.ToList();
                    }
                    catch
                    {
                        Message = "There was an error finding the test results.";
                    }


                //This might get refactored 
                if (CurrentSite.Name != "Default") 
                {
                    var siteTypeQuery = _context.SiteTypes.Where(x => x.SiteTypeID == CurrentSite.SiteTypeID).Select(x => x.Name);
                    this.CurrentSiteType = siteTypeQuery.First();

                }
            //}
        }
        public void OnPost()
        {

        }
    }
}
