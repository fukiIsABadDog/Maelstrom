using EF_Models.Models;
using Maelstrom.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Maelstrom
{
    /// <summary>
    /// Test code for User Page
    /// </summary>
     public class UserHomeModel : PageModel
    {
        private readonly MaelstromContext _context;

        public UserHomeModel(MaelstromContext context)
        {

            _context = context;
        }

        public string Message { get; private set; } = "PageModel in C#"; // razor page example text
        
        public AppUser? ThisAppUser { get; private set; }

        public ICollection<Site>? ThisUsersSites { get; private set; }

        public Site? CurrentSite { get; private set; }

        public ICollection<TestResult>? ThisUsersTestResults  { get; private set; } // Stopped here 3/25 -- pick back up


        //This will need addititional logic for user to save fish to his personal fish collection. As opposed to just the Site "owning" it.
        //public ICollection<Fish>? ThisUsersFish { get; private set; }


     

        public void OnGet()
        {
            if(User.Identity != null)
            {   
                //selects current user
                var currentAppUser = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                this.ThisAppUser = currentAppUser;

                if (ThisAppUser != null)
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
                    var usersSites = querySiteUsers.Select(x => x).Where(x => x.appUser.Id == ThisAppUser.Id).Select(x => x.site).ToList(); 
                    this.ThisUsersSites = usersSites;

                    // This is temperory test logic for current site.. eventually we need to be able to toggle it
                    var currentSite = ThisUsersSites.FirstOrDefault();
                    if (currentSite != null)
                    {
                        this.CurrentSite = currentSite;
                    }
                    else
                    {
                       // new empty site
                    }


                    // ThisUsersTestResults goes here


                }


            }
        }
           
    }
}
