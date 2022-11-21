using AutoMapper;
using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models.FarmModels;
using InnoGotchi_WebApi.Models.PetModels;
using InnoGotchi_WebApi.Models.UserModels;
using System.Security.Claims;

namespace InnoGotchi_WebApi.Services.FarmService
{
    public class FarmService : IFarmService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public FarmService(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Farm CreateFarm(HttpContext httpContext, FarmCreateDto request)
        {
            var user = GetUserByContext(httpContext);

            if (_db.Farms.Any(f => f.UserId == user.Id))
            {
                throw new Exception("You already have a farm.");
            }

            if (_db.Farms.Any(f => f.Name == request.Name))
            {
                throw new Exception("This name is already taken.");
            }

            Farm farm = _mapper.Map<Farm>(request);
            farm.User = user;

            _db.Add(farm);
            _db.SaveChanges();
            
            return farm;
        }

        public FarmDetailsDto GetDetails(HttpContext httpContext)
        {
            var farm = GetFarmByContext(httpContext);

            int petCount = _db.Pets.Count(p => p.Farm.Id == farm.Id);
            int aliveCount = _db.Pets.Count(p => p.Farm.Id == farm.Id && p.IsAlive);
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

        public List<Farm> GetFriendsFarms(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }

        public List<Pet> GetPets(HttpContext httpContext)
        {
            var farm = GetFarmByContext(httpContext);
            
            return _db.Pets.Where(p => p.Farm.Id == farm.Id).ToList();
        }

        public User AddFriend(HttpContext httpContext)
        {
            throw new NotImplementedException();
        }

        private Farm GetFarmByContext(HttpContext httpContext)
        {
            var user = GetUserByContext(httpContext);

            var farm = _db.Farms.FirstOrDefault(f => f.UserId == user.Id);
            if (farm == null)
            {
                throw new Exception("You don't have a farm.");
            }

            return farm;
        }
        private User GetUserByContext(HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            return user;
        }
    }
}
