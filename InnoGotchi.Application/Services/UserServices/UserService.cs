using InnoGotchi.Application.Dtos.UserModels;
using InnoGotchi.Application.Interfaces;
using InnoGotchi.Application.Services.UserServices;
using InnoGotchi.Domain.Entities;

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
        
        public User GetDetails()
        {
            return _userRepository.GetDetails();
        }

        public User ChangePassword(ChangePasswordDto input)
        {
            var user = _userRepository.ChangePassword(input);

            return user;
        }

        public User ChangeUsername(ChangeUsernameDto input)
        {
            var user = _userRepository.ChangeUsername(input);

            return user;
        }

        public void ChangeAvatar(string fileName)
        {
            _userRepository.ChangeAvatar(fileName);
        }

        public string RefreshToken(string secretKey)
        {
            var token = _userRepository.RefreshToken(secretKey);

            return token;
        }
    }
}
