using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EF_Models.Models
{
    public class AppUser : IdentityUser
    {


        public string FirstName {get;set;} = null!;
        
       public string LastName {get;set;} = null!;
       public ICollection<SiteUser>? SiteUsers { get; set; }



    }
}
