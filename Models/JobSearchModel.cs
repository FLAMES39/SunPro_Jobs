namespace SunPro_Jobs.Models
{
    public class JobSearchModel
    {
        public string? JobName { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsDeleted { get; set; }
        public string? JobType { get; set; }
    }
}
