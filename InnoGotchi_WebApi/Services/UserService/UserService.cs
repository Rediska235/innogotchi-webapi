using AutoMapper;
using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models.UserModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InnoGotchi_WebApi.Services.UserService
{
    public class UserService : IUserService
    {
        Exception alreadyExists = new Exception("The user with this email already exists.");
        Exception invalidPassword = new Exception("Invalid password.");
        Exception passNotMatch = new Exception("New passwords do not match.");
        Exception wrongPassword = new Exception("Wrong password.");
        Exception userNotFound = new Exception("User not found.");

        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public UserService(AppDbContext context, IMapper mapper, IConfiguration configuration, IWebHostEnvironment environment)
        {
            _db = context;
            _mapper = mapper;
            _configuration = configuration;
            _environment = environment;
        }

        public UserService()
        {
        }

        public User Register(UserRegisterDto request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user != null)
            {
                throw alreadyExists;
            }

            user = _mapper.Map<User>(request);
            user.CreatedAt = DateTime.Now;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordLength = request.Password.Length;

            _db.Add(user);
            _db.SaveChanges();

            return user;
        }
        
        public string Login(UserLoginDto request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                throw userNotFound;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!isValidPassword)
            {
                throw wrongPassword;
            }

            string secretKey = _configuration["Jwt:Key"];
            string token = CreateToken(user, secretKey);
            
            return token;
        }

        public string CreateToken(User user, string secretKey)
        {
            var claims = new List<Claim>
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
            return Utils.GetUserByContext(_db, httpContext);
        }

        public User ChangePassword(HttpContext httpContext, ChangePasswordDto input)
        {
            var user = Utils.GetUserByContext(_db, httpContext);

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(input.OldPassword, user.PasswordHash);
            if (!isValidPassword)
            {
                throw invalidPassword;
            }

            if (input.NewPassword != input.ConfirmNewPassword)
            {
                throw passNotMatch; 
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(input.NewPassword);
            user.PasswordLength = input.NewPassword.Length;

            _db.Update(user);
            _db.SaveChanges();

            return user;
        }
        
        public User ChangeUsername(HttpContext httpContext, ChangeUsernameDto input)
        {
            var user = Utils.GetUserByContext(_db, httpContext);

            user.FirstName = input.FirstName;
            user.LastName = input.LastName;

            _db.Update(user);
            _db.SaveChanges();

            return user;
        }
        
        public void ChangeAvatar(HttpContext httpContext, string fileName)
        {
            var user = Utils.GetUserByContext(_db, httpContext);
            user.AvatarFileName = fileName;

            _db.Update(user);
            _db.SaveChanges();
        }
    }
}
