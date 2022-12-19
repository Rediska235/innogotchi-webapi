using AutoMapper;
using InnoGotchi.Application.Dtos.UserModels;
using InnoGotchi.Application.Exceptions;
using InnoGotchi.Application.Interfaces;
using InnoGotchi.Domain;
using InnoGotchi.Domain.Entities;
using InnoGotchi.Infrastructure.Data;
using InnoGotchi.Infrastructure.UtilsFolder;
using Microsoft.AspNetCore.Http;

namespace InnoGotchi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        AlreadyExistsException userAlreadyExists = new AlreadyExistsException("The user with this email already exists.");
        NotFoundException userNotFound = new NotFoundException("User not found.");
        NotAllowedException invalidRefreshToken = new NotAllowedException("Invalid refresh token.");
        NotAllowedException refreshTokenExpired = new NotAllowedException("Refresh token expired.");
        InvalidPasswordException invalidPassword = new InvalidPasswordException("Invalid password.");
        InvalidPasswordException passNotMatch = new InvalidPasswordException("New passwords do not match.");
        InvalidPasswordException wrongPassword = new InvalidPasswordException("Wrong password.");

        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(AppDbContext db, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
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
           
            string token = JwtManager.CreateToken(user, secretKey);

            RefreshToken refreshToken = JwtManager.GenerateRefreshToken();
            JwtManager.SetRefreshToken(refreshToken, _httpContextAccessor.HttpContext, user);
            _db.SaveChanges();

            return token;
        }
        
        public string RefreshToken(string secretKey)
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var user = _db.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
            if (user == null)
            {
                throw invalidRefreshToken;
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                throw refreshTokenExpired;
            }

            string token = JwtManager.CreateToken(user, secretKey);
            
            var newRefreshToken = JwtManager.GenerateRefreshToken();
            JwtManager.SetRefreshToken(newRefreshToken, _httpContextAccessor.HttpContext, user);
            _db.SaveChanges();

            return token;
        }
        
        public User GetDetails()
        {
            return Utils.GetUserByContext(_db, _httpContextAccessor.HttpContext);
        }

        public User ChangePassword(ChangePasswordDto input)
        {
            var user = Utils.GetUserByContext(_db, _httpContextAccessor.HttpContext);

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

        public User ChangeUsername(ChangeUsernameDto input)
        {
            var user = Utils.GetUserByContext(_db, _httpContextAccessor.HttpContext);

            user.FirstName = input.FirstName;
            user.LastName = input.LastName;

            _db.Users.Update(user);
            _db.SaveChanges();

            return user;
        }

        public void ChangeAvatar(string fileName)
        {
            var user = Utils.GetUserByContext(_db, _httpContextAccessor.HttpContext);
            user.AvatarFileName = fileName;

            _db.Users.Update(user);
            _db.SaveChanges();
        }
    }
}
