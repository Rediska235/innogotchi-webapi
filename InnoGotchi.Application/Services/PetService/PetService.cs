using InnoGotchi.Application.Dtos.PetModels;
using InnoGotchi.Application.Interfaces;
using InnoGotchi.Domain.Entities;

namespace InnoGotchi.Application.Services.PetService
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;

        public PetService(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public Pet CreatePet(PetCreateDto request)
        {
            var pet = _petRepository.CreatePet(request);

            return pet;
        }

        public Pet ChangeName(PetChangeNameDto request)
        {
            var pet = _petRepository.ChangeName(request);

            return pet;
        }

        public Pet GetDetails(int id)
        {
            var pet = _petRepository.GetDetails(id);

            return pet;
        }

        public List<Pet> GetAllPets()
        {
            var pets = _petRepository.GetAllPets();

            return pets;
        }

        public Pet GiveFood(int id)
        {
            var pet = _petRepository.GiveFood(id);

            return pet;
        }

        public Pet GiveWater(int id)
        {
            var pet = _petRepository.GiveWater(id);

            return pet;
        }
    }
}
