
using InnoGotchi.Application.Dtos.PetModels;
using InnoGotchi.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InnoGotchi.Application.Services.PetService
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
