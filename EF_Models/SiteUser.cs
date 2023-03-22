using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Models.Models
{
    public class SiteUser
    {
        public int SiteUserID { get; set; } // probably will be removed onced composite key is fixed
        public int SiteID { get; set; }

        public AppUser? AppUser { get; set; } 
        public Site? Site { get; set; }
           
    }
}