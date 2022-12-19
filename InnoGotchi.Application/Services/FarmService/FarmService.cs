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
            return _farmRepository.CreateFarm(request);
        }

        public Farm ChangeName(FarmCreateDto request)
        {
            return _farmRepository.ChangeName(request);
        }

        public FarmDetailsDto GetDetails()
        {
            return _farmRepository.GetDetails();
        }

        public List<Pet> GetPets()
        {
            return _farmRepository.GetPets();
        }

        public User AddFriend(string email)
        {
            return _farmRepository.AddFriend(email);
        }

        public List<Farm> GetFriendsFarms()
        {
            return _farmRepository.GetFriendsFarms();
        }
    }
}
