﻿using AutoMapper;
using InnoGotchi_WebApi.Models.Farm;
using InnoGotchi_WebApi.Models.User;

namespace InnoGotchi_WebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegisterDto, User>();
            CreateMap<FarmCreateDto, Farm>();
        }
    }
}
