using InnoGotchi.Application.Exceptions;
using InnoGotchi.Domain.Entities;
using InnoGotchi.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InnoGotchi.Infrastructure.UtilsFolder
{
    public class Utils
    {
        static NotAllowedException dontHaveFarm = new NotAllowedException("You don't have a farm.");
        static NotFoundException petNotFound = new NotFoundException("Pet not found.");

        public static User GetUserByContext(AppDbContext db, HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            string email = identity.FindFirst(ClaimTypes.Email).Value;
            var user = db.Users
                .Include(u => u.FriendsFarms)
                .ThenInclude(ff => ff.Farm)
                .ThenInclude(f => f.Pets)
                .Include(u => u.Farm)
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
