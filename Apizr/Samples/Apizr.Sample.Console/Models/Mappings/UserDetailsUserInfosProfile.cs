using Apizr.Sample.Models;
using AutoMapper;

namespace Apizr.Sample.Console.Models.Mappings
{
    public class UserDetailsUserInfosProfile : Profile
    {
        public UserDetailsUserInfosProfile()
        {
            CreateMap<UserDetails, UserInfos>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.User.Avatar))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Ad.Company))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Ad.Url))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Ad.Text));
        }
    }
}
