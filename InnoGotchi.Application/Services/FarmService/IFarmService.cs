using InnoGotchi.Application.Dtos.FarmModels;
using InnoGotchi.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InnoGotchi.Application.Services.FarmService
{
    public interface IFarmService
    {
        Farm CreateFarm(HttpContext httpContext, FarmCreateDto request);
        Farm ChangeName(HttpContext httpContext, FarmCreateDto request);
        FarmDetailsDto GetDetails(HttpContext httpContext);
        List<Pet> GetPets(HttpContext httpContext);
        List<Farm> GetFriendsFarms(HttpContext httpContext);
        User AddFriend(HttpContext httpContext, string email);
    }
}
