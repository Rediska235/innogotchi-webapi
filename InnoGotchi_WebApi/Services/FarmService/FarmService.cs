using AutoMapper;
using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models.Farm;
using InnoGotchi_WebApi.Models.Pet;
using InnoGotchi_WebApi.Models.User;
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

            Farm farm = _mapper.Map<Farm>(request);
            farm.User = user;

            _db.Add(farm);
            _db.SaveChanges();
            
            return farm;
        }

        public Farm GetDetails(HttpContext httpContext)
        {
            var farm = GetFarmByContext(httpContext);

            return farm;
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
