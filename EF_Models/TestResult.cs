namespace EF_Models.Models
{
    public class TestResult
    {
        public int TestResultID { get; set; }
        public DateTime? CreationDate { get; set; }
        public float? Temperature { get; set; }
        public float? Ph { get; set; }
        public decimal? Sality { get; set; }
        public decimal? Alkalinty { get; set; }
        public decimal? Calcium { get; set; }
        public decimal? Magnesium { get; set; }
        public decimal? Phosphate { get; set; }
        public decimal? Nitrate { get; set; }
        public decimal? Nitrite { get; set; }
        public decimal? Ammonia { get; set; }

        public int SiteUserID { get; set; }
        public SiteUser? SiteUser { get; set; }





    }
}