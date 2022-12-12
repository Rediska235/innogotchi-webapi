using InnoGotchi.Application.Dtos.FarmModels;
using InnoGotchi.Application.Interfaces;
using InnoGotchi.Domain.Entities;

namespace InnoGotchi.Application.Services.FarmService
{
    public class FarmService : IFarmService
    {
        private readonly IFarmRepository _farmRepository;

        public FarmService(IFarmRepository farmRepository)
        {
            _farmRepository = farmRepository;
        }

        public Farm CreateFarm(FarmCreateDto request)
        {
            var farm = _farmRepository.CreateFarm(request);

            return farm;
        }

        public Farm ChangeName(FarmCreateDto request)
        {
            var farm = _farmRepository.ChangeName(request);

            return farm;
        }

        public FarmDetailsDto GetDetails()
        {
            FarmDetailsDto result = _farmRepository.GetDetails();

            return result;
        }

        public List<Pet> GetPets()
        {
            var pets = _farmRepository.GetPets();

            return pets;
        }

        public User AddFriend(string email)
        {
            var friend = _farmRepository.AddFriend(email);

            return friend;
        }

        public List<Farm> GetFriendsFarms()
        {
            var farms = _farmRepository.GetFriendsFarms();

            return farms;
        }
    }
}
