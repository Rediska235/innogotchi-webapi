using InnoGotchi.Application.Dtos.UserModels;
using InnoGotchi.Application.Services.UserServices;
using InnoGotchi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi.WebApi.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegisterDto request)
        {
            User result;
            try
            {
                result = _userService.Register(request);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {
            string secretKey = _configuration.GetSection("JWT:Key").Value;
            string result;
            try
            {
                result = _userService.Login(request, secretKey);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
            return Ok(result);
        }

        [HttpGet("getDetails"), Authorize]
        public async Task<ActionResult<User>> GetDetails()
        {
            return Ok(_userService.GetDetails(HttpContext));
        }

        [HttpPut("changePassword"), Authorize]
        public async Task<ActionResult<User>> ChangePassword(ChangePasswordDto input)
        {
            User result;
            try
            {
                result = _userService.ChangePassword(HttpContext, input);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(result);
        }
        
        [HttpPut("changeUsername"), Authorize]
        public async Task<ActionResult<User>> ChangeUsername(ChangeUsernameDto input)
        {
            return Ok(_userService.ChangeUsername(HttpContext, input));
        }
        
        [HttpPut("changeAvatar/{fileName}"), Authorize]
        public async Task<ActionResult> ChangeAvatar(string fileName)
        {
            _userService.ChangeAvatar(HttpContext, fileName);
            return Ok();
        }
    }
}
