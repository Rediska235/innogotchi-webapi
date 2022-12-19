using InnoGotchi.Application.Dtos.PetModels;
using InnoGotchi.Domain.Entities;

namespace InnoGotchi.Application.Interfaces
{
    public interface IPetRepository
    {

        Pet CreatePet(PetCreateDto request);
        Pet ChangeName(PetChangeNameDto request);
        Pet GetDetails(int id);
        List<Pet> GetAllPets();
        Pet GiveFood(int id);
        Pet GiveWater(int id);
    }
}
