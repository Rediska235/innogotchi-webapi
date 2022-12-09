using InnoGotchi.Application.Dtos.UserModels;
using InnoGotchi.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InnoGotchi.Application.Services.UserServices
{
    public interface IUserService
    {
        User Register(UserRegisterDto request);
        string Login(UserLoginDto request, string secretKey);
        User GetDetails(HttpContext httpContext);
        User ChangePassword(HttpContext httpContext, ChangePasswordDto input);
        User ChangeUsername(HttpContext httpContext, ChangeUsernameDto input);
        void ChangeAvatar(HttpContext httpContext, string fileName);
    }
}
