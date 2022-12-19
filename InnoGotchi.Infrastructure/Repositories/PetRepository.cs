using AutoMapper;
using InnoGotchi.Application.Dtos.PetModels;
using InnoGotchi.Application.Exceptions;
using InnoGotchi.Application.Interfaces;
using InnoGotchi.Domain.Entities;
using InnoGotchi.Infrastructure.Data;
using InnoGotchi.Infrastructure.UtilsFolder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InnoGotchi.Infrastructure.Repositories
{
    public class PetRepository : IPetRepository
    {
        NotAllowedException dontHaveFarm = new NotAllowedException("You don't have a farm.");
        AlreadyExistsException takenName = new AlreadyExistsException("This name is already taken.");
        NotAllowedException notYourPet = new NotAllowedException("You cannot interact with this pet.");
        NotAllowedException cantChangeName = new NotAllowedException("You cannot change the name of this pet.");
        NotAllowedException deadPet = new NotAllowedException("This pet is dead.");

        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PetRepository(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public Pet CreatePet(PetCreateDto request)
        {
            var farm = Utils.GetFarmByContext(_db, _httpContextAccessor.HttpContext);
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

            _db.Pets.Add(pet);
            _db.SaveChanges();

            return pet;
        }

        public Pet ChangeName(PetChangeNameDto request)
        {
            var pet = Utils.GetPetById(_db, request.Id);

            var user = Utils.GetUserByContext(_db, _httpContextAccessor.HttpContext);
            if (!IsMyPet(user, pet))
            {
                throw cantChangeName;
            }

            if (_db.Pets.Any(p => p.Name == request.Name))
            {
                throw takenName;
            }
            
            pet.Name = request.Name;
            pet.SetVitalSigns();

            _db.Pets.Update(pet);
            _db.SaveChanges();

            return pet;
        }

        public Pet GetDetails(int id)
        {
            var pet = Utils.GetPetById(_db, id);

            if (!IsMyOrFriendsPet(pet))
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

            foreach (var pet in pets)
            {
                pet.SetVitalSigns();
            }

            return pets;
        }

        public Pet GiveFood(int id)
        {
            var pet = Utils.GetPetById(_db, id);

            if (!IsMyOrFriendsPet(pet))
            {
                throw notYourPet;
            }

            if (!pet.IsAlive)
            {
                throw deadPet;
            }

            pet.LastFed = DateTime.Now;
            pet.SetVitalSigns();

            _db.Pets.Update(pet);
            _db.SaveChanges();

            return pet;
        }

        public Pet GiveWater(int id)
        {
            var pet = Utils.GetPetById(_db, id);

            if (!IsMyOrFriendsPet(pet))
            {
                throw notYourPet;
            }

            if (!pet.IsAlive)
            {
                throw deadPet;
            }

            pet.LastDrank = DateTime.Now;
            pet.SetVitalSigns();

            _db.Pets.Update(pet);
            _db.SaveChanges();

            return pet;
        }

        private bool IsMyOrFriendsPet(Pet pet)
        {
            var user = Utils.GetUserByContext(_db, _httpContextAccessor.HttpContext);
            var friendsFarms = user.FriendsFarms.Select(ff => ff.Farm).ToList();

            if (friendsFarms.Any(f => f.Pets.Contains(pet)))
            {
                return true;
            }

            return IsMyPet(user, pet);
        }

        private bool IsMyPet(User user, Pet pet)
        {
            Farm farm = user.Farm;

            if(farm == null)
            {
                return false;
            }

            if (farm.Pets.Contains(pet))
            {
                return true;
            }

            return false;
        }
    }
}
