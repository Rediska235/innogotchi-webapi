using AutoMapper;
using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models;
using InnoGotchi_WebApi.Models.FarmModels;
using InnoGotchi_WebApi.Models.PetModels;
using InnoGotchi_WebApi.Models.UserModels;

namespace InnoGotchi_WebApi.Services.FarmService
{
    public class FarmService : IFarmService
    {
        Exception takenName = new Exception("This name is already taken.");
        Exception alreadyHaveFarm = new Exception("You already have a farm.");
        Exception userNotFound = new Exception("User not found.");
        Exception alreadyFrinds = new Exception("You are already friends with this user.");
        
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public FarmService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Farm CreateFarm(HttpContext httpContext, FarmCreateDto request)
        {
            var user = Utils.GetUserByContext(_db, httpContext);

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

            _db.Add(farm);
            _db.SaveChanges();
            
            return farm;
        }

        public Farm ChangeName(HttpContext httpContext, FarmCreateDto request)
        {
            var farm = Utils.GetFarmByContext(_db, httpContext);
            farm.Name = request.Name;

            _db.Update(farm);
            _db.SaveChanges();

            return farm;
        }

        public FarmDetailsDto GetDetails(HttpContext httpContext)
        {
            var farm = Utils.GetFarmByContext(_db, httpContext);

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

        public List<Pet> GetPets(HttpContext httpContext)
        {
            var farm = Utils.GetFarmByContext(_db, httpContext);
            
            return farm.Pets;
        }

        public User AddFriend(HttpContext httpContext, string email)
        {
            var farm = Utils.GetFarmByContext(_db, httpContext);

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

        public List<Farm> GetFriendsFarms(HttpContext httpContext)
        {
            var user = Utils.GetUserByContext(_db, httpContext);

            return user.FriendsFarms.Select(ff => ff.Farm).ToList();
        }
    }
}
