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

        public string Message { get; private set; } = "PageModel in C#";
        
        public AppUser? ThisAppUser { get; private set; }

        public ICollection<Site>? ThisUsersSites { get; private set; }

        public ICollection<TestResult>? ThisUsersTestResults  { get; private set; }


        //This will need addititional logic for user to save fish to his personal fish collection. As opposed to just the Site "owning" it.
        //public ICollection<Fish>? ThisUsersFish { get; private set; }


     

        public void OnGet()
        {
            if(User.Identity != null)
            {
                //var currentUser = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                //if (currentUser != null) 
                //{
                //    Message += $" The person loged in is: {currentUser.LastName}";
                //}


                var currentAppUser = _context.Users.FirstOrDefault(x => x.UserName == User.Identity.Name);
                this.ThisAppUser = currentAppUser;

                if (ThisAppUser != null)
                {
                    

                    var querySiteUsers = from SiteUser in _context.SiteUsers
                                     join Sites in _context.Sites on SiteUser.SiteID equals Sites.SiteID
                                     select new
                                     {
                                         site = SiteUser.Site,
                                         appUser = SiteUser.AppUser
                                     };


                    var usersSites = querySiteUsers.Select(x => x).Where(x => x.appUser.Id == ThisAppUser.Id).Select(x => x.site).ToList(); 

                    this.ThisUsersSites = usersSites;



                }

                //foreach (var site in querySiteUsers)
                //{
                //    if(site.appUser == ThisAppUser)
                //    {
                       
                //    }
                //}



                


            }
        }
           
    }
}
