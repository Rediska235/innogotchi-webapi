using InnoGotchi.Application.Dtos.PetModels;
using InnoGotchi.Application.Services.PetService;
using InnoGotchi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InnoGotchi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpPost("createPet")]
        public async Task<ActionResult<Pet>> CreatePet(PetCreateDto request)
        {
            Pet pet;
            try
            {
                pet = _petService.CreatePet(request);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(pet);
        }
        
        [HttpPut("changeName")]
        public async Task<ActionResult<Pet>> ChangeName(PetChangeNameDto request)
        {
            Pet pet;
            try
            {
                pet = _petService.ChangeName(request);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(pet);
        }


        [HttpGet("getDetails/{id}"), Authorize]
        public async Task<ActionResult<Pet>> GetDetails(int id)
        {
            Pet pet;
            try
            {
                pet = _petService.GetDetails(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
            return Ok(pet);
        }

        [HttpGet("getAllPets"), Authorize]
        public async Task<ActionResult<List<Pet>>> GetAllPets()
        {
            return Ok(_petService.GetAllPets());
        }

        [HttpPut("giveFood/{id}"), Authorize]
        public async Task<ActionResult<Pet>> GiveFood(int id)
        {
            Pet pet;
            try
            {
                pet = _petService.GiveFood(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(pet);
        }

        [HttpPut("giveWater/{id}"), Authorize]
        public async Task<ActionResult<Pet>> GiveWater(int id)
        {
            Pet pet;
            try
            {
                pet = _petService.GiveWater(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(pet);
        }
    }
}
