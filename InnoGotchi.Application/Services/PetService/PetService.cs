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
            return _petRepository.CreatePet(request);
        }

        public Pet ChangeName(PetChangeNameDto request)
        {
            return _petRepository.ChangeName(request);
        }

        public Pet GetDetails(int id)
        {
            return _petRepository.GetDetails(id);
        }

        public List<Pet> GetAllPets()
        {
            return _petRepository.GetAllPets();
        }

        public Pet GiveFood(int id)
        {
            return _petRepository.GiveFood(id);
        }

        public Pet GiveWater(int id)
        {
            return _petRepository.GiveWater(id);
        }
    }
}
