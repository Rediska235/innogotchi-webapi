using Moq;
using AutoMapper;
using AutoFixture;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using InnoGotchi.Infrastructure;
using InnoGotchi.Domain.Entities;
using InnoGotchi.Infrastructure.Data;
using InnoGotchi.Application.Exceptions;
using InnoGotchi.Application.Dtos.PetModels;
using InnoGotchi.Infrastructure.Repositories;

namespace InnoGotchi.UnitTests
{
    public class PetRepositoryTests
    {
        private Fixture _fixture;
        private PetRepository _petRepository;
        
        private AppDbContext _db;
        private Mock<IMapper> _mapper;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        
        private Farm farm;
        private Pet pet;
        private User user;

        public PetRepositoryTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            EntitiesInitialization();

            DependenciesInitialization();
        }

        private void EntitiesInitialization()
        {
            user = _fixture.Create<User>();
            farm = _fixture.Create<Farm>();
            pet = _fixture.Create<Pet>();
            
            farm.UserId = user.Id;
        }

        private void DependenciesInitialization()
        {
            _db = InMemoryDatabaseBuilder.Build();
            _db.Farms.Add(farm);
            _db.Users.Add(user);
            _db.SaveChanges();

            _mapper = new Mock<IMapper>();

            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Email, user.Email),
            });

            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _httpContextAccessor.Setup(x => x.HttpContext.User.Identity).Returns(identity);
        }

        [Fact]
        public void CreatePet_ReturnsPet()
        {
            // Arrange
            var petCreateDto = _fixture.Create<PetCreateDto>();

            _mapper.Setup(x => x.Map<Pet>(petCreateDto)).Returns(pet);

            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            // Act
            var result = _petRepository.CreatePet(petCreateDto);

            // Assert
            Assert.IsType<Pet>(result);
        }
        
        [Fact]
        public void CreatePet_ThrowsException_WhenDbIsEmpty()
        {
            // Arrange
            var petCreateDto = _fixture.Create<PetCreateDto>();

            _mapper.Setup(x => x.Map<Pet>(petCreateDto)).Returns(pet);
            
            // Database is empty
            _db = InMemoryDatabaseBuilder.Build();
            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            // Act
            
            //Assert
            Assert.Throws<InvalidOperationException>(() => { var result = _petRepository.CreatePet(petCreateDto); });
        }

        [Fact]
        public void CreatePet_ThrowsException_WhenUserDontHaveFarm()
        {
            // Arrange
            var petCreateDto = _fixture.Create<PetCreateDto>();

            _mapper.Setup(x => x.Map<Pet>(petCreateDto)).Returns(pet);

            farm = _fixture.Create<Farm>();
            // There is no connection between user and farm (farm.UserId = user.Id;)
            
            _db = InMemoryDatabaseBuilder.Build();
            _db.Farms.Add(farm);
            _db.Users.Add(user);
            _db.SaveChanges();

            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            // Act

            //Assert
            Assert.Throws<NotAllowedException>(() => { var result = _petRepository.CreatePet(petCreateDto); });
        }

        [Fact]
        public void ChangeName_ReturnsPet()
        {
            // Arrange
            var petChangeNameDto = _fixture.Create<PetChangeNameDto>();
            petChangeNameDto.Id = pet.Id;

            user.Farm = farm;
            pet.Farm = farm;

            _db = InMemoryDatabaseBuilder.Build();
            _db.Pets.Add(pet);
            _db.Users.Add(user);
            _db.SaveChanges();

            _mapper.Setup(x => x.Map<Pet>(petChangeNameDto)).Returns(pet);

            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            // Act
            var result = _petRepository.ChangeName(petChangeNameDto);

            // Assert
            Assert.IsType<Pet>(result);
        }

        [Fact]
        public void ChangeName_ThrowsException_CannotChangeNameWhenItsNotMyPet()
        {
            // Arrange
            var petChangeNameDto = _fixture.Create<PetChangeNameDto>();
            petChangeNameDto.Id = pet.Id;

            // There is no connection between user and pet (user.Farm = farm; pet.Farm = farm;)

            _db = InMemoryDatabaseBuilder.Build();
            _db.Pets.Add(pet);
            _db.Users.Add(user);
            _db.SaveChanges();

            _mapper.Setup(x => x.Map<Pet>(petChangeNameDto)).Returns(pet);

            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            // Act

            // Assert
            Assert.Throws<NotAllowedException>(() => { var result = _petRepository.ChangeName(petChangeNameDto); });
        }

        [Fact]
        public void ChangeName_ThrowsException_WhenPetDoesntExist()
        {
            // Arrange
            var petChangeNameDto = _fixture.Create<PetChangeNameDto>();
            // petChangeNameDto.Id != pet.Id -> pet with this id doesnt exist thats causing an exception
            petChangeNameDto.Id = pet.Id + 1;

            user.Farm = farm;
            pet.Farm = farm;

            _db = InMemoryDatabaseBuilder.Build();
            _db.Pets.Add(pet);
            _db.Users.Add(user);
            _db.SaveChanges();

            _mapper.Setup(x => x.Map<Pet>(petChangeNameDto)).Returns(pet);

            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            // Act

            // Assert
            Assert.Throws<NotFoundException>(() => { var result = _petRepository.ChangeName(petChangeNameDto); });
        }
        
        [Fact]
        public void ChangeName_ThrowsException_WhenNameIsTaken()
        {
            // Arrange
            user.Farm = farm;
            var pet1 = pet;
            pet1.Farm = farm;
            var pet2 = _fixture.Create<Pet>();
            pet2.Farm = farm;

            _db = InMemoryDatabaseBuilder.Build();
            _db.Pets.Add(pet1);
            _db.Pets.Add(pet2);
            _db.Users.Add(user);
            _db.SaveChanges();

            var petChangeNameDto = new PetChangeNameDto();
            petChangeNameDto.Id = pet2.Id;
            // pet1's Name is already taken
            petChangeNameDto.Name = pet1.Name;

            _mapper.Setup(x => x.Map<Pet>(petChangeNameDto)).Returns(pet2);

            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            // Act

            // Assert
            Assert.Throws<AlreadyExistsException>(() => { var result = _petRepository.ChangeName(petChangeNameDto); });
        }

        [Fact]
        public void GetDetails_ReturnsPet()
        {
            // Arrange
            user.Farm = farm;
            pet.Farm = farm;

            _db = InMemoryDatabaseBuilder.Build();
            _db.Pets.Add(pet);
            _db.Users.Add(user);
            _db.SaveChanges();

            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            // Act
            var result = _petRepository.GetDetails(pet.Id);

            // Assert
            Assert.IsType<Pet>(result);
        }

        [Fact]
        public void GetAllPets_ReturnsListOfPets()
        {
            // Arrange
            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            // Act
            var result = _petRepository.GetAllPets();

            // Assert
            Assert.IsType<List<Pet>>(result);
        }

        [Fact]
        public void GiveFood_ReturnsPet()
        {
            // Arrange
            user.Farm = farm;
            pet.Farm = farm;
            pet.IsAlive = true;

            _db = InMemoryDatabaseBuilder.Build();
            _db.Pets.Add(pet);
            _db.Users.Add(user);
            _db.SaveChanges();

            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            DateTime now = DateTime.UtcNow;
            long nowSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

            // Act
            var result = _petRepository.GiveFood(pet.Id);

            long petLastFedSeconds = new DateTimeOffset(pet.LastFed).ToUnixTimeSeconds();

            // Assert
            Assert.IsType<Pet>(result);
            Assert.Equal(petLastFedSeconds, nowSeconds);
        }

        [Fact]
        public void GiveFood_ThrowsException_WhenPetIsDead()
        {
            // Arrange
            user.Farm = farm;
            pet.Farm = farm;
            // Pet is dead (pet.IsAlive = false)
            pet.IsAlive = false;

            _db = InMemoryDatabaseBuilder.Build();
            _db.Pets.Add(pet);
            _db.Users.Add(user);
            _db.SaveChanges();

            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            // Act
            
            // Assert
            Assert.Throws<NotAllowedException>(() => { var result = _petRepository.GiveFood(pet.Id); });
        }

        [Fact]
        public void GiveWater_ReturnsPet()
        {
            // Arrange
            user.Farm = farm;
            pet.Farm = farm;
            pet.IsAlive = true;

            _db = InMemoryDatabaseBuilder.Build();
            _db.Pets.Add(pet);
            _db.Users.Add(user);
            _db.SaveChanges();

            _petRepository = new PetRepository(_db, _mapper.Object, _httpContextAccessor.Object);

            DateTime now = DateTime.UtcNow;
            long nowSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

            // Act
            var result = _petRepository.GiveWater(pet.Id);

            long petLastDrankSeconds = new DateTimeOffset(pet.LastDrank).ToUnixTimeSeconds();

            // Assert
            Assert.IsType<Pet>(result);
            Assert.Equal(petLastDrankSeconds, nowSeconds);
        }
    }
}
