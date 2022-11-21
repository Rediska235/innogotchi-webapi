using AutoMapper;
using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models.PetModels;
using System.Security.Claims;

namespace InnoGotchi_WebApi.Services.PetService
{
    public class PetService : IPetService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public PetService(AppDbContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public Pet CreatePet(HttpContext httpContext, PetCreateDto request)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            var farm = _db.Farms.FirstOrDefault(f => f.UserId == user.Id);
            if (farm == null)
            {
                throw new Exception("You don't have a farm.");
            }

            if (_db.Pets.Any(p => p.Name == request.Name))
            {
                throw new Exception("This name is already taken.");
            }

            Pet pet = _mapper.Map<Pet>(request);
            pet.Farm = farm;

            _db.Add(pet);
            _db.SaveChanges();

            return pet;
        }

        public Pet GetDetails(int id)
        {
            Pet pet = _db.Pets.FirstOrDefault(p => p.Id == id);
            if (pet == null)
            {
                throw new Exception("Pet not found.");
            }

            pet.SetVitalSigns();

            return pet;
        }

        public List<Pet> GetAllPets()
        {
            var pets = _db.Pets.OrderBy(p => p.HappinessDays).ToList();
            foreach(Pet pet in pets)
            {
                pet.SetVitalSigns();
            }
            
            return pets;
        }

        public Pet GiveFood(int id)
        {
            Pet pet = _db.Pets.FirstOrDefault(p => p.Id == id);
            if (pet == null)
            {
                throw new Exception("Pet not found.");
            }

            pet.LastFed = DateTime.Now;
            
            _db.Update(pet);
            _db.SaveChanges();

            return pet;
        }

        public Pet GiveWater(int id)
        {
            Pet pet = _db.Pets.FirstOrDefault(p => p.Id == id);
            if (pet == null)
            {
                throw new Exception("Pet not found.");
            }
            
            pet.LastDrank = DateTime.Now;
            
            _db.Update(pet);
            _db.SaveChanges();
            
            return pet;
        }
    }
}
