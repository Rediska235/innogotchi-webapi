using AutoMapper;
using InnoGotchi.Application.Dtos.FarmModels;
using InnoGotchi.Application.Exceptions;
using InnoGotchi.Application.Interfaces;
using InnoGotchi.Domain.Entities;
using InnoGotchi.Infrastructure.Data;
using InnoGotchi.Infrastructure.UtilsFolder;
using Microsoft.AspNetCore.Http;

namespace InnoGotchi.Infrastructure.Repositories
{
    public class FarmRepository : IFarmRepository
    {
        NotFoundException userNotFound = new NotFoundException("User not found.");
        AlreadyExistsException takenName = new AlreadyExistsException("This name is already taken.");
        AlreadyExistsException alreadyHaveFarm = new AlreadyExistsException("You already have a farm.");
        AlreadyExistsException alreadyFrinds = new AlreadyExistsException("You are already friends with this user.");

        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FarmRepository(AppDbContext db, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public Farm CreateFarm(FarmCreateDto request)
        {
            var user = Utils.GetUserByContext(_db, _httpContextAccessor.HttpContext);

            if (_db.Farms.Any(f => f.UserId == user.Id))
            {
                throw alreadyHaveFarm;
            }

            if (_db.Farms.Any(f => f.Name == request.Name))
            {
                throw takenName;
            }

            var farm = _mapper.Map<Farm>(request);
            farm.User = user;
            farm.UserId = user.Id;
            farm.Pets = new List<Pet>();

            _db.Farms.Add(farm);    
            _db.SaveChanges();

            return farm;
        }

        public Farm ChangeName(FarmCreateDto request)
        {
            var farm = Utils.GetFarmByContext(_db, _httpContextAccessor.HttpContext);
            farm.Name = request.Name;

            _db.Farms.Update(farm);
            _db.SaveChanges();

            return farm;
        }

        public FarmDetailsDto GetDetails()
        {
            var farm = Utils.GetFarmByContext(_db, _httpContextAccessor.HttpContext);

            int petCount = farm.Pets.Count();
            int aliveCount = farm.Pets.Count(p => p.IsAlive);
            int deadCount = petCount - aliveCount;

            FarmDetailsDto result = new FarmDetailsDto
            {
                Name = farm.Name,
                CreatedAt = farm.CreatedAt,
                PetCount = petCount,
                AliveCount = aliveCount,
                DeadCount = deadCount
            };

            return result;
        }

        public List<Pet> GetPets()
        {
            var farm = Utils.GetFarmByContext(_db, _httpContextAccessor.HttpContext);

            return farm.Pets;
        }

        public User AddFriend(string email)
        {
            var farm = Utils.GetFarmByContext(_db, _httpContextAccessor.HttpContext);

            var friend = _db.Users.FirstOrDefault(u => u.Email == email);
            if (friend == null)
            {
                throw userNotFound;
            }

            if (farm.Friends.Any(ff => ff.UserId == friend.Id))
            {
                throw alreadyFrinds;
            }

            _db.FriendFarms.Add(new FriendFarm
            {
                User = friend,
                Farm = farm
            });
            _db.SaveChanges();

            return friend;
        }

        public List<Farm> GetFriendsFarms()
        {
            var user = Utils.GetUserByContext(_db, _httpContextAccessor.HttpContext);
            
            return user.FriendsFarms.Select(ff => ff.Farm).ToList();
        }
    }
}
