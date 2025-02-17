using System.ComponentModel.DataAnnotations;

namespace SunPro_Jobs.Models
{
    public class UserModel
    {


        [Required(ErrorMessage =  "Name Is Required")]
        public string Names { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]       
        public string Password { get; set; }


        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]      
        public string Email { get; set; }


        [Required(ErrorMessage = "PhoneNumber Is Required")]
        public string PhoneNumber { get; set; }

    }
}
