using AutoMapper;
using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InnoGotchi_WebApi.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _db = context;
            _mapper = mapper;
            _configuration = configuration;
        }


        public User Register(UserRegisterDto request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user != null)
            {
                throw new Exception("The user with this email already exists.");
            }

            user = _mapper.Map<User>(request);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            _db.Add(user);
            _db.SaveChanges();

            return user;
        }
        
        public string Login(UserLoginDto request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValidPassword)
            {
                throw new Exception("Wrong password.");
            }

            string secretKey = _configuration["Jwt:Key"];
            string token = CreateToken(user, secretKey);
            
            return token;
        }

        public string CreateToken(User user, string secretKey)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public User GetDetails(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            return user;
        }

        public User ChangePassword(HttpContext httpContext, ChangePasswordDto input)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);
            
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(input.OldPassword, user.PasswordHash);
            if (!isValidPassword)
            {
                throw new Exception("Invalid password.");
            }

            if (input.NewPassword != input.ConfirmNewPassword)
            {
                throw new Exception("Passwords do not match."); 
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.NewPassword);

            _db.Update(user);
            _db.SaveChanges();

            return user;
        }
        
        public User ChangeUsername(HttpContext httpContext, ChangeUsernameDto input)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            user.FirstName = input.FirstName;
            user.LastName = input.LastName;

            _db.Update(user);
            _db.SaveChanges();

            return user;
        }
    }
}
