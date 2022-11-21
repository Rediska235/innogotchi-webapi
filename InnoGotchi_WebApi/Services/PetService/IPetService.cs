using InnoGotchi_WebApi.Models.PetModels;

namespace InnoGotchi_WebApi.Services.PetService
{
    public interface IPetService
    {
        Pet CreatePet(HttpContext httpContext, PetCreateDto request);
        Pet GetDetails(int id);
        List<Pet> GetAllPets();

        Pet GiveFood(int id);
        Pet GiveWater(int id);
    }
}
