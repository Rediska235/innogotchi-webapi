using InnoGotchi.Application.Dtos.FarmModels;
using InnoGotchi.Application.Interfaces;
using InnoGotchi.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InnoGotchi.Application.Services.FarmService
{
    public class FarmService : IFarmService
    {
        private readonly IFarmRepository _farmRepository;

        public FarmService(IFarmRepository farmRepository)
        {
            _farmRepository = farmRepository;
        }

        public Farm CreateFarm(HttpContext httpContext, FarmCreateDto request)
        {
            var farm = _farmRepository.CreateFarm(httpContext, request);

            return farm;
        }

        public Farm ChangeName(HttpContext httpContext, FarmCreateDto request)
        {
            var farm = _farmRepository.ChangeName(httpContext, request);

            return farm;
        }

        public FarmDetailsDto GetDetails(HttpContext httpContext)
        {
            FarmDetailsDto result = _farmRepository.GetDetails(httpContext);

            return result;
        }

        public List<Pet> GetPets(HttpContext httpContext)
        {
            var pets = _farmRepository.GetPets(httpContext);

            return pets;
        }

        public User AddFriend(HttpContext httpContext, string email)
        {
            var friend = _farmRepository.AddFriend(httpContext, email);

            return friend;
        }

        public List<Farm> GetFriendsFarms(HttpContext httpContext)
        {
            var farms = _farmRepository.GetFriendsFarms(httpContext);

            return farms;
        }
    }
}
