using InnoGotchi.Application.Dtos.FarmModels;
using InnoGotchi.Domain.Entities;

namespace InnoGotchi.Application.Interfaces
{
    public interface IFarmRepository
    {
        Farm CreateFarm(FarmCreateDto request);
        Farm ChangeName(FarmCreateDto request);
        FarmDetailsDto GetDetails();
        List<Pet> GetPets();
        List<Farm> GetFriendsFarms();
        User AddFriend(string email);
    }
}
