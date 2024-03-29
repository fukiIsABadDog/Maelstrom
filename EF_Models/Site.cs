using System.ComponentModel.DataAnnotations;

namespace EF_Models.Models
{
    public class Site
    {
        public int SiteID { get; set; }
        public string Name { get; set; } = null!;
        public int? Capacity { get; set; }
        public string? Location { get; set; }
        public byte[]? ImageData { get; set; }
        public int SiteTypeID { get; set; }
        public SiteType? SiteType { get; set; }
        public ICollection<SiteUser>? SiteUsers { get; set; }
        public ICollection<Fish>? Fishs { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Deleted { get; set; }

    }
}