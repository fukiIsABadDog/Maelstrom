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

        
        public string CurrentSiteType { get; private set; } = string.Empty;
        public AppUser? CurrentAppUser { get; private set; }

        public ICollection<Site>? CurrentUserSites { get; private set; }


        [BindProperty(SupportsGet = true)]
        public Site CurrentSite { get; set; }   

        //extention types will change
        public string SiteImage { get; set; }

        public ICollection<TestResult>? CurrentSiteTestResults  { get; private set; } 

        //This will need addititional logic for user to save fish to his personal fish collection. As opposed to just the Site "owning" it.
        //public ICollection<Fish>? ThisUsersFish { get; private set; }

        public void OnGet(Site currentSite)
        {

            CurrentAppUser = _appUserService.FindAppUser(User.Identity); 
            CurrentUserSites = _appUserService.CurrentUserSites(CurrentAppUser);
            CurrentSite = _appUserService.SelectedSite(CurrentUserSites, currentSite);
            CurrentSiteTestResults = _appUserService.SelectedSiteTestResults(CurrentSite);
                

                //This might get refactored into a enum --  
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
