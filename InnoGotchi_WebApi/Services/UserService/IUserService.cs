using InnoGotchi_WebApi.Models.UserModels;

namespace InnoGotchi_WebApi.Services.UserService
{
    public interface IUserService
    {
        User Register(UserRegisterDto request);
        string Login(UserLoginDto request);
        string CreateToken(User user, string secretKey);
        User GetDetails(HttpContext httpContext);
        User ChangePassword(HttpContext httpContext, ChangePasswordDto input);
        User ChangeUsername(HttpContext httpContext, ChangeUsernameDto input);
        void ChangeAvatar(HttpContext httpContext, string fileName);
    }
}
