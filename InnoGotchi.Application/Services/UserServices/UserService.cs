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
            return _userRepository.Register(request);
        }

        public string Login(UserLoginDto request, string secretKey)
        {
            return _userRepository.Login(request, secretKey);
        }
        
        public User GetDetails()
        {
            return _userRepository.GetDetails();
        }

        public User ChangePassword(ChangePasswordDto input)
        {
            return _userRepository.ChangePassword(input);
        }

        public User ChangeUsername(ChangeUsernameDto input)
        {
            return _userRepository.ChangeUsername(input);
        }

        public void ChangeAvatar(string fileName)
        {
            _userRepository.ChangeAvatar(fileName);
        }

        public string RefreshToken(string secretKey)
        {
            return _userRepository.RefreshToken(secretKey);
        }
    }
}
