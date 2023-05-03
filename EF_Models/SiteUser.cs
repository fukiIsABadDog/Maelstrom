using System.ComponentModel.DataAnnotations;

namespace EF_Models.Models
{
    public class SiteUser
    {
        public int SiteUserID { get; set; } // probably will be removed onced composite key is fixed
        public int SiteID { get; set; }
        public bool IsAdmin { get; set; } = false;

        public AppUser? AppUser { get; set; }
        public Site? Site { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Deleted { get; set; }

    }
}