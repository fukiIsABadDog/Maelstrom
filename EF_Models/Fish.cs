using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFcoreTesting.Models
{
    public class Fish
    {
        public int FishID { get; set; }
        public string? NameOrTag { get; set; }
        public DateTime? DateAdded { get; set; }

        public int FishTypeID { get; set; }
        public FishType? FishType { get; set; }

        public int SiteID { get; set; }
        public Site? Site { get; set; } 

        public byte? Image {get; set;} // we may want to host the images on a seperate file server
                                        // in that case we may want to just have the URL here

    }
}