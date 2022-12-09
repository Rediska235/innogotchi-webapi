using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InnoGotchi.Application.Services.FarmService;
using InnoGotchi.Application.Dtos.FarmModels;
using InnoGotchi.Domain.Entities;

namespace InnoGotchi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        private readonly IFarmService _farmService;

        public FarmController(IFarmService farmService)
        {
            _farmService = farmService;
        }

        [HttpPost("createFarm"), Authorize]
        public async Task<ActionResult<Farm>> CreateFarm(FarmCreateDto request)
        {
            Farm result;
            try
            {
                result = _farmService.CreateFarm(HttpContext, request);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(result);
        }
        
        [HttpPut("changeName"), Authorize]
        public async Task<ActionResult<Farm>> ChangeName(FarmCreateDto request)
        {
            Farm result;
            try
            {
                result = _farmService.ChangeName(HttpContext, request);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(result);
        }

        [HttpGet("getDetails"), Authorize]
        public async Task<ActionResult<FarmDetailsDto>> GetDetails()
        {
            FarmDetailsDto result;
            try
            {
                result = _farmService.GetDetails(HttpContext);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(result);
        }

        [HttpGet("getPets"), Authorize]
        public async Task<ActionResult<List<Pet>>> GetPets()
        {
            List<Pet> result;
            try
            {
                result = _farmService.GetPets(HttpContext);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(result);
        }

        [HttpPost("addFriend"), Authorize]
        public async Task<ActionResult<string>> AddFriend(string email)
        {
            User result;
            try
            {
                result = _farmService.AddFriend(HttpContext, email);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok($"Friend {result.FirstName} {result.LastName} added");
        }

        [HttpGet("getFriendsFarms"), Authorize]
        public async Task<ActionResult<List<Farm>>> GetFriendsFarms()
        {
            List<Farm> result;
            try
            {
                result = _farmService.GetFriendsFarms(HttpContext);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(result);
        }
    }
}
