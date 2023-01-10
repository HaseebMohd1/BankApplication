using AutoMapper;
using WebApplication1.DTO;
using Bank.Models;

namespace WebApplication1
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<User, UserDto>();
            CreateMap<Employee, EmployeeDto>();
        }
    }
}
