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
       
        private readonly IAppUserService _appUserService;
        public UserHomeModel(IAppUserService appUserService)
        {   
            _appUserService = appUserService;     
        }

        public string CurrentSiteType { get; set; } = string.Empty;
        public AppUser? CurrentAppUser { get; private set; }
        public ICollection<Site>? CurrentUserSites { get; private set; }
        [BindProperty(SupportsGet = true)]
        public Site CurrentSite { get; private set; }  
        
        // -- this feature is still in development --
        public string SiteImage { get; private set; }
        public ICollection<TestResult>? CurrentSiteTestResults  { get; private set; } 

        //This will need addititional logic for user to save fish to his personal fish collection. As opposed to just the Site "owning" it.
        //public ICollection<Fish>? ThisUsersFish { get; private set; }

        public void OnGet(Site currentSite)
        {
            //This block calls custom services to set page properities
            CurrentAppUser = _appUserService.FindAppUser(User.Identity); 
            CurrentUserSites = _appUserService.CurrentUserSites(CurrentAppUser);
            CurrentSite = _appUserService.SelectedSite(CurrentUserSites, currentSite);
            CurrentSiteTestResults = _appUserService.SelectedSiteTestResults(CurrentSite);
            CurrentSiteType = _appUserService.GetSiteType(CurrentSite);   

            // may want to create another method for handling maintance after the table is created in DB
        }
        public void OnPost()
        {

        }
    }
}
