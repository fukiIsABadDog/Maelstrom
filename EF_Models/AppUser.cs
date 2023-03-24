using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Models.Models
{
    public class AppUser : IdentityUser
    {
        // still trying to figure out properties
        [Required]
       public string FirstName {get;set;} = null!;
        [Required]
       public string LastName {get;set;} = null!;
       public ICollection<SiteUser>? SiteUsers { get; set; }

       //public ICollection<TestResult>? TestResults { get; set; } 

       //public int AccountID { get;set;}
       //public Account? Account { get; set; }


        
    }
}