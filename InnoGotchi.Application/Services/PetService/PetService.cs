using AutoMapper;
using InnoGotchi.Application.Dtos.PetModels;
using InnoGotchi.Application.Interfaces;
using InnoGotchi.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InnoGotchi.Application.Services.PetService
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetService(IPetRepository petRepository, IMapper mapper)
        {
            _petRepository = petRepository;
        }

        public Pet CreatePet(HttpContext httpContext, PetCreateDto request)
        {
            var pet = _petRepository.CreatePet(httpContext, request);

            return pet;
        }

        public Pet ChangeName(HttpContext httpContext, PetChangeNameDto request)
        {
            var pet = _petRepository.ChangeName(httpContext, request);

            return pet;
        }

        public Pet GetDetails(HttpContext httpContext, int id)
        {
            var pet = _petRepository.GetDetails(httpContext, id);

            return pet;
        }

        public List<Pet> GetAllPets()
        {
            var pets = _petRepository.GetAllPets();

            return pets;
        }

        public Pet GiveFood(HttpContext httpContext, int id)
        {
            var pet = _petRepository.GiveFood(httpContext, id);

            return pet;
        }

        public Pet GiveWater(HttpContext httpContext, int id)
        {
            var pet = _petRepository.GiveWater(httpContext, id);

            return pet;
        }
    }
}
