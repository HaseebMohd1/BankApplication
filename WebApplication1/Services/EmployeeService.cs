using AutoMapper;
using WebApplication1.DTO;
using WebApplication1.Repository;

namespace WebApplication1.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }


        public Task<List<UserDto>> GetUsers()
        {
            var response = _employeeRepository.GetUsers();

            return response;
        }


        public UserDto GetUserById(int userId)
        {
            try
            {
                if (!this.isUserActive(userId))
                {
                    throw new Exception("User Doesn't Exits!!!");
                }


                var userDetails = _employeeRepository.GetUserDetails(userId);

                var response = _mapper.Map<UserDto>(userDetails);

                return response;

            }
            catch
            {
                throw new Exception("Error : Couldn't GET user details by given ID");
            }
        }



        public bool isUserActive(int userId)
        {

            var userDetails = _employeeRepository.GetUserDetails(userId);

            if(userDetails == null)
            {
                return false;

            }

            if(userDetails != null && userDetails.IsActive==0)
            {
                return false;
            }

            return true;
        }
    }
}
