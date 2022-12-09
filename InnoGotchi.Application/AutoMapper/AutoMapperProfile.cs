using AutoMapper;
using InnoGotchi.Application.Dtos.FarmModels;
using InnoGotchi.Application.Dtos.PetModels;
using InnoGotchi.Application.Dtos.UserModels;
using InnoGotchi.Domain.Entities;

namespace InnoGotchi.Application.AutoMapper
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
