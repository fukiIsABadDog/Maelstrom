using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Models.Models
{
    public class SiteType 
    {
        public int SiteTypeID { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<Site>? Sites {get; set;}

    }
}