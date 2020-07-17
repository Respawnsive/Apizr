using System;
using System.Collections.Generic;
using System.Text;
using Apizr.Sample.Api.Models;
using AutoMapper;

namespace Apizr.Sample.Console.Models.Mappings
{
    public class UserMinUserProfile : Profile
    {
        public UserMinUserProfile()
        {
            CreateMap<User, MinUser>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FirstName));

            CreateMap<MinUser, User>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
