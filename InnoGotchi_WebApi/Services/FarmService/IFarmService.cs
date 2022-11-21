using InnoGotchi_WebApi.Models.FarmModels;
using InnoGotchi_WebApi.Models.PetModels;
using InnoGotchi_WebApi.Models.UserModels;

namespace InnoGotchi_WebApi.Services.FarmService
{
    public interface IFarmService
    {
        Farm CreateFarm(HttpContext httpContext, FarmCreateDto request);
        FarmDetailsDto GetDetails(HttpContext httpContext);
        List<Pet> GetPets(HttpContext httpContext);
        List<Farm> GetFriendsFarms(HttpContext httpContext);
        User AddFriend(HttpContext httpContext);
    }
}
