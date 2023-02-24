using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Models.Models
{
    public class SiteUser
    {
        public int UserID { get; set; }
        public int SiteID { get; set; }

        public User? User { get; set; } 
        public Site? Site { get; set; }
           
    }
}