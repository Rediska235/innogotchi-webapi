using InnoGotchi.Application.Dtos.UserModels;
using InnoGotchi.Domain.Entities;

namespace InnoGotchi.Application.Services.UserServices
{
    public interface IUserService
    {
        User Register(UserRegisterDto request);
        string Login(UserLoginDto request, string secretKey);
        string RefreshToken(string secretKey);
        User GetDetails();
        User ChangePassword(ChangePasswordDto input);
        User ChangeUsername(ChangeUsernameDto input);
        void ChangeAvatar(string fileName);
    }
}
