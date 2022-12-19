using AutoFixture;
using InnoGotchi.Application.Dtos.PetModels;
using InnoGotchi.Application.Services.PetService;
using InnoGotchi.Domain.Entities;
using InnoGotchi.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InnoGotchi.UnitTests
{
    public class PetControllerTests
    {
        private Mock<IPetService> _petService;
        private Fixture _fixture;
        private PetController _petController;

        public PetControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            
            _petService = new Mock<IPetService>();
        }

        [Fact]
        public async void CreatePet_ReturnsOK()
        {
            // Arrange
            var pet = _fixture.Create<Pet>();
            _petService.Setup(x => x.CreatePet(It.IsAny<PetCreateDto>())).Returns(pet);
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.CreatePet(_fixture.Create<PetCreateDto>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void CreatePet_ThrowsException()
        {
            // Arrange
            _petService.Setup(x => x.CreatePet(It.IsAny<PetCreateDto>())).Throws(new Exception());
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.CreatePet(_fixture.Create<PetCreateDto>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    
        [Fact]
        public async void ChangeName_ReturnsOK()
        {
            // Arrange
            var pet = _fixture.Create<Pet>();
            _petService.Setup(x => x.ChangeName(It.IsAny<PetChangeNameDto>())).Returns(pet);
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.ChangeName(_fixture.Create<PetChangeNameDto>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void ChangeName_ThrowsException()
        {
            // Arrange
            _petService.Setup(x => x.ChangeName(It.IsAny<PetChangeNameDto>())).Throws(new Exception());
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.ChangeName(_fixture.Create<PetChangeNameDto>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void GetDetails_ReturnsOK()
        {
            // Arrange
            var pet = _fixture.Create<Pet>();
            _petService.Setup(x => x.GetDetails(It.IsAny<int>())).Returns(pet);
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.GetDetails(_fixture.Create<int>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        [Fact]
        public async void GetDetails_ThrowsException()
        {
            // Arrange
            _petService.Setup(x => x.GetDetails(It.IsAny<int>())).Throws(new Exception());
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.GetDetails(_fixture.Create<int>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void GetAllPets_ReturnsOK()
        {
            // Arrange
            var pets = _fixture.CreateMany<Pet>(3).ToList();
            _petService.Setup(x => x.GetAllPets()).Returns(pets);
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.GetAllPets();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GiveFood_ReturnsOK()
        {
            // Arrange
            var pet = _fixture.Create<Pet>();
            _petService.Setup(x => x.GiveFood(It.IsAny<int>())).Returns(pet);
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.GiveFood(_fixture.Create<int>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GiveFood_ThrowsException()
        {
            // Arrange
            _petService.Setup(x => x.GiveFood(It.IsAny<int>())).Throws(new Exception());
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.GiveFood(_fixture.Create<int>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async void GiveWater_ReturnsOK()
        {
            // Arrange
            var pet = _fixture.Create<Pet>();
            _petService.Setup(x => x.GiveWater(It.IsAny<int>())).Returns(pet);
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.GiveWater(_fixture.Create<int>());

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async void GiveWater_ThrowsException()
        {
            // Arrange
            _petService.Setup(x => x.GiveWater(It.IsAny<int>())).Throws(new Exception());
            _petController = new PetController(_petService.Object);

            // Act
            var result = await _petController.GiveWater(_fixture.Create<int>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}
