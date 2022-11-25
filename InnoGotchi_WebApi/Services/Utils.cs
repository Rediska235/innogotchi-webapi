using InnoGotchi_WebApi.Data;
using InnoGotchi_WebApi.Models.FarmModels;
using InnoGotchi_WebApi.Models.PetModels;
using InnoGotchi_WebApi.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InnoGotchi_WebApi.Services
{
    public class Utils
    {
        static Exception dontHaveFarm = new Exception("You don't have a farm.");
        static Exception petNotFound = new Exception("Pet not found.");

        public static User GetUserByContext(AppDbContext db, HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = db.Users
                .Include(u => u.FriendsFarms)
                .ThenInclude(ff => ff.Farm)
                .ThenInclude(f => f.Pets)
                .FirstOrDefault(u => u.Email == email);

            return user;
        }

        public static Farm GetFarmByContext(AppDbContext db, HttpContext httpContext)
        {
            var user = GetUserByContext(db, httpContext);

            var farm = db.Farms
                .Include(f => f.Pets)
                .Include(f => f.Friends)
                .FirstOrDefault(f => f.UserId == user.Id);
            
            if (farm == null)
            {
                throw dontHaveFarm;
            }

            return farm;
        }

        public static Pet GetPetById(AppDbContext db, int id)
        {
            var pet = db.Pets
                .Include(p => p.Farm)
                .FirstOrDefault(p => p.Id == id);
            
            if (pet == null)
            {
                throw petNotFound;
            }

            return pet;
        }
    }
}
