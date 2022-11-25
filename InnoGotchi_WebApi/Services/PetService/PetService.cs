using AutoMapper;
using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models.PetModels;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchi_WebApi.Services.PetService
{
    public class PetService : IPetService
    {
        Exception dontHaveFarm = new Exception("You don't have a farm.");
        Exception takenName = new Exception("This name is already taken.");
        Exception notYourPet = new Exception("You cannot interact with this pet.");
        Exception deadPet = new Exception("Your pet is dead.");

        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public PetService(AppDbContext context, IMapper mapper)
        {
            _db = context;
            _mapper = mapper;
        }

        public Pet CreatePet(HttpContext httpContext, PetCreateDto request)
        {
            var farm = Utils.GetFarmByContext(_db, httpContext);
            if (farm == null)
            {
                throw dontHaveFarm;
            }

            if (_db.Pets.Any(p => p.Name == request.Name))
            {
                throw takenName;
            }

            var pet = _mapper.Map<Pet>(request);
            pet.Farm = farm;

            _db.Add(pet);
            _db.SaveChanges();

            return pet;
        }

        public Pet ChangeName(HttpContext httpContext, PetChangeNameDto request)
        {
            var pet = Utils.GetPetById(_db, request.Id);
            
            if (!IsMyOrFriendsPet(httpContext, pet))
            {
                throw notYourPet;
            }
            
            pet.Name = request.Name;
            pet.SetVitalSigns();

            _db.Update(pet);
            _db.SaveChanges();

            return pet;
        }

        public Pet GetDetails(HttpContext httpContext, int id)
        {
            var pet = Utils.GetPetById(_db, id);
            
            if (!IsMyOrFriendsPet(httpContext, pet))
            {
                throw notYourPet;
            }

            pet.SetVitalSigns();

            return pet;
        }
        
        public List<Pet> GetAllPets()
        {
            var pets = _db.Pets
                .Include(p => p.Farm)
                .OrderBy(p => p.HappinessDays)
                .ToList();
            
            foreach(var pet in pets)
            {
                pet.SetVitalSigns();
            }
            
            return pets;
        }
        
        public Pet GiveFood(HttpContext httpContext, int id)
        {
            var pet = Utils.GetPetById(_db, id);

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
            var pet = Utils.GetPetById(_db, id);
            
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
            var user = Utils.GetUserByContext(_db, httpContext);
            var farm = Utils.GetFarmByContext(_db, httpContext);
            
            if (farm.Pets.Contains(pet))
            {
                return true;
            }
            
            var friendsFarms = user.FriendsFarms.Select(ff => ff.Farm).ToList();
            
            if (friendsFarms.Any(f => f.Pets.Contains(pet)))
            {
                return true;
            }

            return false;
        }
    }
}
