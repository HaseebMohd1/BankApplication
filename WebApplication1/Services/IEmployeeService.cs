using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public interface IEmployeeService
    {
        public Task<List<UserDto>> GetUsers();

        public bool isUserActive(int userId);

        public UserDto GetUserById(int userId);
    }
}
