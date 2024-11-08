﻿using System.ComponentModel.DataAnnotations;

namespace Portfolio.API.DTOS.Users
{
    public class RegistrationDTO
    {


        public string? FName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@".*@adminDKK\.com$", ErrorMessage = "Invalid email.")]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }



        [Required]
        public string PhoneNumber { get; set; }

        


    }
}
