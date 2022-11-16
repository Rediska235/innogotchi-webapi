using AutoMapper;
using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InnoGotchi_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public UserController(IConfiguration configuration, AppDbContext db, IMapper mapper)
        {
            _configuration = configuration;
            _db = db;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegisterDto request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == request.Email);

            if(user != null)
            {
                return BadRequest("The user with this email already exists.");
            }

            user = _mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            
            _db.Add(user);
            _db.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValidPassword)
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:Key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        [HttpGet("getDetails"), Authorize]
        public async Task<ActionResult<User>> GetDetails()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            return Ok(user);
        }

        [HttpPost("changePassword"), Authorize]
        public async Task<ActionResult<User>> ChangePassword(ChangePasswordDto input)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(input.OldPassword, user.PasswordHash);
            if (!isValidPassword)
            {
                return BadRequest("Wrong old password.");
            }

            if (input.NewPassword != input.ConfirmNewPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.NewPassword);

            _db.Update(user);
            _db.SaveChanges();

            return Ok(user);
        }
        
        [HttpPost("changeUsername"), Authorize]
        public async Task<ActionResult<User>> ChangeUsername(ChangeUsernameDto input)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            user.FirstName = input.FirstName;
            user.LastName = input.LastName;

            _db.Update(user);
            _db.SaveChanges();

            return Ok(user);
        }
    }
}
