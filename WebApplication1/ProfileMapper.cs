using AutoMapper;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<User, UserDto>();
        }
    }
}
