using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.DTOS.Users;
using Portfolio.API.Errors;
using Portfolio.Core.Identity;
using Portfolio.Core.Services.Interfaces;
using System.Data;
using System.Security.Claims;

namespace Portfolio.API.Controllers
{
   
    public class AccountController :  APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager,ITokenService  tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }



        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                await _userManager.AddToRoleAsync(user, "Admin");

            
               var response = new UserResponseDTO()
                {
                    UserName = model.UserName,
                    UserEmail = model.Email,
                    Token =  await _tokenService.CreateTokenAsync(user , _userManager),
                    
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
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault(); // Assuming a user has one role


                var response = new UserResponseDTO()
                {
                    UserName = user.UserName,
                    UserEmail = user.Email,
                    Role = role,
                    Token = await _tokenService.CreateTokenAsync(user, _userManager)
                };

                return Ok(response);
            }
            else
            {
                return Unauthorized(new ApiResponse(401, "Invalid Email or Password"));
            }
        }


        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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



        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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



        [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("getCurrentUser")]
        public async Task<ActionResult<UserResponseDTO>> GetCurrentUser()
        {
            var EmailUser = User.FindFirstValue(ClaimTypes.Email); // Cannot return null because he must be login and have token

            var userData = await _userManager.FindByEmailAsync(EmailUser);


            var roles = await _userManager.GetRolesAsync(userData);
            var role = roles.FirstOrDefault(); // Assuming a user has one role

            return Ok(new UserResponseDTO()
            {
                UserName = userData.UserName,
                UserEmail = userData.Email,
                Role = role,
                Token = await _tokenService.CreateTokenAsync(userData, _userManager)
            }
            );
        }

    }
}
