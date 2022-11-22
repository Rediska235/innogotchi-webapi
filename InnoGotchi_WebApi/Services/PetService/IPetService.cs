using InnoGotchi_WebApi.Models.PetModels;

namespace InnoGotchi_WebApi.Services.PetService
{
    public interface IPetService
    {
        Pet CreatePet(HttpContext httpContext, PetCreateDto request);
        Pet ChangeName(HttpContext httpContext, PetChangeNameDto request);
        Pet GetDetails(HttpContext httpContext, int id);
        List<Pet> GetAllPets();

        Pet GiveFood(HttpContext httpContext, int id);
        Pet GiveWater(HttpContext httpContext, int id);
    }
}
