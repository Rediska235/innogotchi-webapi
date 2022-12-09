using AutoMapper;
using InnoGotchi.Application.Dtos.UserModels;
using InnoGotchi.Application.Exceptions;
using InnoGotchi.Application.Interfaces;
using InnoGotchi.Domain.Entities;
using InnoGotchi.Infrastructure.Data;
using InnoGotchi.Infrastructure.UtilsFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace InnoGotchi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        AlreadyExistsException userAlreadyExists = new AlreadyExistsException("The user with this email already exists.");
        NotFoundException userNotFound = new NotFoundException("User not found.");
        InvalidPasswordException invalidPassword = new InvalidPasswordException("Invalid password.");
        InvalidPasswordException passNotMatch = new InvalidPasswordException("New passwords do not match.");
        InvalidPasswordException wrongPassword = new InvalidPasswordException("Wrong password.");

        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UserRepository(AppDbContext db, IMapper mapper, IConfiguration config)
        {
            _db = db;
            _mapper = mapper;
            _config = config;
        }

        public User Register(UserRegisterDto request)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user != null)
            {
                throw userAlreadyExists;
            }

            user = _mapper.Map<User>(request);
            user.CreatedAt = DateTime.Now;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            user.PasswordLength = request.Password.Length;

            _db.Users.Add(user);
            _db.SaveChanges();

            return user;
        }

        public string Login(UserLoginDto request, string secretKey)
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

            _db.Users.Update(user);
            _db.SaveChanges();

            return user;
        }

        public User ChangeUsername(HttpContext httpContext, ChangeUsernameDto input)
        {
            var user = Utils.GetUserByContext(_db, httpContext);

            user.FirstName = input.FirstName;
            user.LastName = input.LastName;

            _db.Users.Update(user);
            _db.SaveChanges();

            return user;
        }

        public void ChangeAvatar(HttpContext httpContext, string fileName)
        {
            var user = Utils.GetUserByContext(_db, httpContext);
            user.AvatarFileName = fileName;

            _db.Users.Update(user);
            _db.SaveChanges();
        }
    }
}
