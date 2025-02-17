namespace SunPro_Jobs.Models
{
    public class ApplicationModel
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        public string ResumePath { get; set; }  // Change to string
        public string CoverLetter { get; set; }  // Change to string
        public string City { get; set; }
        public string Street { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string? TemporaryPassword { get; set; }
        public DateTime? TempPasswordExpiry { get; set; }
      
    }
}
