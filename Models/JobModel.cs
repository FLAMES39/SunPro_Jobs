using System.ComponentModel.DataAnnotations;

namespace SunPro_Jobs.Models
{
    public class JobModel
    {
        
        public int JobId { get; set; }

        [Required(ErrorMessage = "Job Name Is Required")]
        public string JobName { get; set; }

        [Required(ErrorMessage = "Job Description Is Required")]
        public string JobDescription { get; set; } = string.Empty;

        public string JobStatus { get; set; } = "Open";

        [Required(ErrorMessage = "Job Type Is Required")]
        public string JobType { get; set; }

        public string Jobink { get; set; } = string.Empty;

        [Required(ErrorMessage = "Job Requirements Are Required")]
        public string JobRequirements { get; set; }

        [Required(ErrorMessage = "Location Is Required")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salary Range Is Required")]
        public string SalaryRange { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required(ErrorMessage = "Posting Date Required")]
        public DateTime PostingDate { get; set; }

        [Required(ErrorMessage = "Closing Date Is Required")]
        public DateTime ClosingDate { get; set; }

       /// public int CompanyId { get; set; }


        //public Company Company { get; set; }

        //public ICollection<Application> Applications {get; set;}
    }
}

