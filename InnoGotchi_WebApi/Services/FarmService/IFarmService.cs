using InnoGotchi_WebApi.Models.Farm;

namespace InnoGotchi_WebApi.Services.FarmService
{
    public interface IFarmService
    {
        Farm CreateFarm(HttpContext httpContext, FarmCreateDto request);
    }
}
