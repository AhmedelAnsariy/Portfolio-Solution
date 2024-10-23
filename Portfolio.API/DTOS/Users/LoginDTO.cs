using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.DTOS.Users
{
    public class LoginDTO
    {


        [Required]
        [RegularExpression(@".*@adminDKK\.com$", ErrorMessage = "Invalid email or password")]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }



    }
}
