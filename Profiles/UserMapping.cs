using AutoMapper;
using epp_be.Data;
using epp_be.DTOs;

namespace epp_be.Profiles
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<RegisterUserDTO, User>();
            CreateMap<User,UserReponseDTO>();
        }

    }
}
