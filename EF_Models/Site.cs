using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFcoreTesting.Models
{
    public class Site
    {
        public int SiteID { get; set; }
        public string Name { get; set; } = null!;
        public int? Capacity { get; set; } 
        public string? Location{ get; set; }

        public int SiteTypeID { get; set; }
        public SiteType? SiteType {get; set;}
        public ICollection<SiteUser>? SiteUsers { get; set; }
        public ICollection<Fish>? Fishs { get; set; }
        public ICollection<TestResult>? TestResults { get; set; }
    }
}