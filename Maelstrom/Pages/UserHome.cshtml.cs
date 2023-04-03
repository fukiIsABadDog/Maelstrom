using EF_Models.Models;
using Maelstrom.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom
{
 
    public class UserHomeModel : PageModel
    {
        private readonly MaelstromContext _context;

        public UserHomeModel(MaelstromContext context)
        {

            _context = context;
        }

        public string Message  {get; set;} = string.Empty;
        public string CurrentSiteType { get; private set; } = string.Empty;
        public AppUser? CurrentAppUser { get; private set; }

        public ICollection<Site>? CurrentUserSites { get; private set; }


        [BindProperty(SupportsGet = true)]
        public Site CurrentSite { get; private set; } = new Site { SiteID = 999, Name= "Default", Capacity = 0, Location = "Does not exist yet"}; 

        public ICollection<TestResult>? CurrentSiteTestResults  { get; private set; } 

        //This will need addititional logic for user to save fish to his personal fish collection. As opposed to just the Site "owning" it.
        //public ICollection<Fish>? ThisUsersFish { get; private set; }

        public void OnGet(Site currentSite)
        {

            if (User.Identity != null)
            {   
                //selects current user
                var currentAppUser = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                this.CurrentAppUser = currentAppUser;

                if (CurrentAppUser != null)
                {
                    //selects a collection of user and site object 
                    var querySiteUsers = from SiteUser in _context.SiteUsers
                                     join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                     select new
                                     {
                                         site = SiteUser.Site,
                                         appUser = SiteUser.AppUser                                     
                                     };

                    //selects only the sites where the SiteUser matches the current user
                    var usersSites = querySiteUsers.Select(x => x).Where(x => x.appUser.Id == CurrentAppUser.Id).Select(x => x.site).ToList(); 
                    this.CurrentUserSites = usersSites;

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
                            this.CurrentSite = firstCurrentSite;

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

                }

                //This might get refactored 
                if (CurrentSite.Name != "Default") 
                {
                    var siteTypeQuery = _context.SiteTypes.Where(x => x.SiteTypeID == CurrentSite.SiteTypeID).Select(x => x.Name);
                    this.CurrentSiteType = siteTypeQuery.First();

                }
            }
        }
        public void OnPost()
        {

        }
    }
}
