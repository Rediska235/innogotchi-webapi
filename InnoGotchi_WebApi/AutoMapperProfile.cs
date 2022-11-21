using AutoMapper;
using InnoGotchi_WebApi.Models.FarmModels;
using InnoGotchi_WebApi.Models.PetModels;
using InnoGotchi_WebApi.Models.UserModels;

namespace InnoGotchi_WebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegisterDto, User>();
            CreateMap<FarmCreateDto, Farm>();
            CreateMap<PetCreateDto, Pet>();
        }
    }
}
