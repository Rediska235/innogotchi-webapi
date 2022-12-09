using InnoGotchi.Application.Dtos.UserModels;
using InnoGotchi.Application.Interfaces;
using InnoGotchi.Application.Services.UserServices;
using InnoGotchi.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InnoGotchi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Register(UserRegisterDto request)
        {
            var user = _userRepository.Register(request);

            return user;
        }

        public string Login(UserLoginDto request, string secretKey)
        {
            var token = _userRepository.Login(request, secretKey);

            return token;
        }
        
        public User GetDetails(HttpContext httpContext)
        {
            return _userRepository.GetDetails(httpContext);
        }

        public User ChangePassword(HttpContext httpContext, ChangePasswordDto input)
        {
            var user = _userRepository.ChangePassword(httpContext, input);

            return user;
        }

        public User ChangeUsername(HttpContext httpContext, ChangeUsernameDto input)
        {
            var user = _userRepository.ChangeUsername(httpContext, input);

            return user;
        }

        public void ChangeAvatar(HttpContext httpContext, string fileName)
        {
            _userRepository.ChangeAvatar(httpContext, fileName);
        }
    }
}
