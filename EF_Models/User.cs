using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFcoreTesting.Models
{
    public class User
    {
        public int UserID {get;set;}
        public string FirstName {get;set;} = null!;
       public string LastName {get;set;} = null!;
       public ICollection<SiteUser>? SiteUsers { get; set; }

       public int AccountID { get;set;}
       public Account? Account { get; set; }


        
    }
}