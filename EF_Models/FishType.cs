using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFcoreTesting.Models
{
    public class FishType
    {
        public int FishTypeID { get; set; }
        public string? CommonName { get; set; } 
        public string? ScientificName {get;set;} 
        public double? MaxSize { get; set; } 
        public int? RecommendedTankSize { get; set; } 
        public double? MinTemp { get; set; } 
        public double? MaxTemp { get; set; } 
        public double? PhMin { get; set; } 
        public double? PhMax { get; set; } 

        public ICollection<Fish>? Fishs {get; set;} 

    }
}