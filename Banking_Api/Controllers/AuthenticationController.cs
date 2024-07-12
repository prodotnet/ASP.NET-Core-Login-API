using Banking_Api.DataTransferObjects.Accounts;
using Banking_Api.Jwt;
using Banking_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Banking_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly JwtServices _jwtService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(JwtServices jwtService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _jwtService = jwtService;
            _signInManager = signInManager;
            _userManager = userManager;
        }
   
        

        [Authorize]

        [HttpPost("UserToken")]
        public async Task<ActionResult<UserDto>> UserToken()
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            return CreateUserDto(user); 
        }



        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized("Invalid Email or Password");

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid Email or Password");

            return CreateUserDto(user);

        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (await CheckEmail(model.Email))
            {
                return BadRequest(new { error = $"The email {model.Email} already exists, Please use a different email address." });
            }




            var newUser = new User
            {
                Firstname = model.FirstName.ToLower(),
                Lastname = model.LastName.ToLower(),
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = true,
                CreatedDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded) return BadRequest(new { errors = result.Errors.Select(e => e.Description).ToArray() });



            return Ok(new JsonResult(new{title = "Account Created", message = "You have successfully registered. You can now sign in "}));
        }


        // Add HTTP method attribute
        [HttpGet("CheckEmail")]
        public async Task<bool> CheckEmail(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email);
        }



        #region Private Helper Methods

        private UserDto CreateUserDto(User user)
        {
            return new UserDto
            {
                FirstName = user.Firstname,
                LastName = user.Lastname,
                Jwt = _jwtService.CreateJWT(user)
            };
        }

        #endregion
    }
}
