using InnoGotchi_WebApi.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InnoGotchi_WebApi.Services.FarmService;

namespace InnoGotchi_WebApi.Controllers
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

        [HttpGet("getDetails"), Authorize]
        public async Task<ActionResult<Farm>> GetDetails()
        {
            Farm result;
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
    }
}
