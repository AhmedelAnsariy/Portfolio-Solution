using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.DTOS.Users;
using Portfolio.API.Errors;
using Portfolio.Core.Identity;

namespace Portfolio.API.Controllers
{
   
    public class AccountController :  APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDTO>> Register(RegistrationDTO model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest(new ApiResponse(400, "Email is already registered."));
            }

            var user = new AppUser()
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Age = model.Age,
                UserName = model.UserName,
                FName = model.FName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var response = new UserResponseDTO()
                {
                    UserName = model.UserName,
                    UserEmail = model.Email,
                    Token = "Token Will Be Here"
                };

                return Ok(response);
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new ApiResponse(400, string.Join(", ", errors)));
            }
        }




        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return Unauthorized(new ApiResponse(401, "Invalid Email or Password"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                var response = new UserResponseDTO()
                {
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    Token = "Token Will Be Here "
                };

                return Ok(response);
            }
            else
            {
                return Unauthorized(new ApiResponse(401, "Invalid Email or Password"));
            }
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponseAllUsers>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userResponseList = users.Select(user => new ResponseAllUsers
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                
            }).ToList();

            return Ok(userResponseList);
        }




        [HttpDelete]
        public async Task<IActionResult> DeleteUserByEmail(DeleteUserDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return NotFound(new ApiResponse(404, "User not found"));
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok(new ApiResponse(200, "User deleted successfully"));
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Failed to delete user"));
            }
        }



    }
}
