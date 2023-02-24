using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFcoreTesting.Models
{
    public class TestResult
    {
        public int TestResultID { get; set;}
        public int SiteID  { get; set; }
        public float? Temperature { get; set;}
        public float? Ph { get; set; }
        public decimal? Sality { get; set; }
        public decimal? Alkalinty { get; set; }
        public decimal? Calcium { get; set; }
        public decimal? Magnesium { get; set; }
        public decimal? Phosphate { get; set; }
        public decimal? Nitrate { get; set; }
        public decimal? Nitrite { get; set; }
        public decimal? Ammonia { get; set; }

        public Site? Site { get; set; } // may be redundant
        public int UserID { get; set; }
        public SiteUser? SiteUser { get; set; }
        
        



    }
}