using AutoMapper;
using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models.Farm;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace InnoGotchi_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FarmController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;

        public FarmController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpPost("createFarm"), Authorize]
        public async Task<ActionResult<Farm>> CreateFarm(FarmCreateDto request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            if (_db.Farms.Any(f => f.UserId == user.Id))
            {
                return BadRequest("You already have a farm.");
            }

            Farm farm = _mapper.Map<Farm>(request);
            farm.User = user;

            _db.Add(farm);
            _db.SaveChanges();

            return Ok(farm);
        }
    }
}
