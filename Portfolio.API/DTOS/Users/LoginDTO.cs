using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.DTOS.Users
{
    public class LoginDTO
    {


        [Required]
        [RegularExpression(@".*@admin\.com$", ErrorMessage = "unvalid email.")]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }



    }
}
