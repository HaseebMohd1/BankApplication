using Bank.Models;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public interface IEmployeeService
    {   
        public Task<List<UserDto>> GetUsers();

        public bool isUserActive(int userId);

        public UserDto GetUserById(int userId);

        public UserDto CreateUser(User userDetails);

        public string EmployeeLogin(string userEmail, string userPassword);
    }
}
