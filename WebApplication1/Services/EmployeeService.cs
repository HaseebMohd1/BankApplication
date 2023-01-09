using WebApplication1.DTO;
using WebApplication1.Repository;

namespace WebApplication1.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }


        public Task<List<UserDto>> GetUsers()
        {
            var response = _employeeRepository.GetUsers();

            

            return response;
        }
    }
}
