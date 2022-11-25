using AutoMapper;
using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models.PetModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InnoGotchi_WebApi.Services.PetService
{
    public class PetService : IPetService
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        Exception dontHaveFarm = new Exception("You don't have a farm.");
        Exception takenName = new Exception("This name is already taken.");
        Exception petNotFound = new Exception("Pet not found.");
        Exception notYourPet = new Exception("You cannot interact with this pet.");
        Exception deadPet = new Exception("Your pet is dead.");

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
                throw dontHaveFarm;
            }

            if (_db.Pets.Any(p => p.Name == request.Name))
            {
                throw takenName;
            }

            Pet pet = _mapper.Map<Pet>(request);
            pet.Farm = farm;

            _db.Add(pet);
            _db.SaveChanges();

            Console.WriteLine("Pet created: " + pet.Name);
            foreach (var p in farm.Pets)
            {
                Console.WriteLine(p.Name);
            }

            return pet;
        }

        public Pet ChangeName(HttpContext httpContext, PetChangeNameDto request)
        {
            Pet pet = GetPetById(request.Id);
            if (!IsMyOrFriendsPet(httpContext, pet))
            {
                throw notYourPet;
            }
            
            pet.Name = request.Name;

            _db.Update(pet);
            _db.SaveChanges();

            return pet;
        }

        public Pet GetDetails(HttpContext httpContext, int id)
        {
            Pet pet = GetPetById(id);
            if (pet == null)
            {
                throw petNotFound;
            }

            if (!IsMyOrFriendsPet(httpContext, pet))
            {
                throw notYourPet;
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
        
        public Pet GiveFood(HttpContext httpContext, int id)
        {
            Pet pet = GetPetById(id);
            if (pet == null)
            {
                throw petNotFound;
            }

            if (!IsMyOrFriendsPet(httpContext, pet))
            {
                throw notYourPet;
            }
            
            if (!pet.IsAlive)
            {
                throw deadPet;
            }

            pet.LastFed = DateTime.Now;
            pet.SetVitalSigns();

            _db.Update(pet);
            _db.SaveChanges();

            return pet;
        }
        
        public Pet GiveWater(HttpContext httpContext, int id)
        {
            Pet pet = GetPetById(id);
            if (pet == null)
            {
                throw petNotFound;
            }
            
            if (!IsMyOrFriendsPet(httpContext, pet))
            {
                throw notYourPet;
            }
            
            if (!pet.IsAlive)
            {
                throw deadPet;
            }

            pet.LastDrank = DateTime.Now;
            pet.SetVitalSigns();

            _db.Update(pet);
            _db.SaveChanges();
            
            return pet;
        }
        
        private bool IsMyOrFriendsPet(HttpContext httpContext, Pet pet)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (pet.Farm.UserId == user.Id)
            {
                return true;
            }

            var friendsFarms = _db.FriendFarms.Where(ff => ff.UserId == user.Id).Select(ff => ff.Farm).ToList();
            if (friendsFarms.Any(f => f.Id == pet.Farm.Id))
            {
                return true;
            }
            
            return false;
        }

        private Pet GetPetById(int id)
        {
            //проверить с Include(p => p.Farm);
            return _db.Pets.Include(p => p.Farm).FirstOrDefault(p => p.Id == id);
        }
    }
}
