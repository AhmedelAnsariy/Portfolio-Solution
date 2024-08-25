using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Portfolio.API.DTOS.Users;
using Portfolio.API.Errors;
using Portfolio.Core.Identity;

namespace Portfolio.API.Controllers
{
   
    public class AccountController :  APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;

        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost("register")]

        public async Task<ActionResult<UserResponseDTO>> Register(RegistrationDTO model)
        {
            var user = new AppUser()
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Age = model.Age,
                UserName = model.UserName,
                FName = model.FName
            };


            var result = await _userManager.CreateAsync(user,model.Password);

            if(result.Succeeded) {
                var response = new UserResponseDTO()
                {
                    UserName = model.UserName,
                    UserEmail = model.Email,
                    Token = "Token Will Be Here "
                };

                return Ok(response);

            }
            else
            {
                return BadRequest(new ApiResponse(400));
            }


        }


    }
}
