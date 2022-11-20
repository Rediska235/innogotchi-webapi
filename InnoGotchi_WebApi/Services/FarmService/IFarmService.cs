using InnoGotchi_WebApi.Models.Farm;
using InnoGotchi_WebApi.Models.Pet;
using InnoGotchi_WebApi.Models.User;

namespace InnoGotchi_WebApi.Services.FarmService
{
    public interface IFarmService
    {
        Farm CreateFarm(HttpContext httpContext, FarmCreateDto request);
        Farm GetDetails(HttpContext httpContext);
        List<Pet> GetPets(HttpContext httpContext);
        List<Farm> GetFriendsFarms(HttpContext httpContext);
        User AddFriend(HttpContext httpContext);
    }
}
