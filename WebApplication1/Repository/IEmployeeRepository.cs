using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IEmployeeRepository
    {
        public Task<List<User>> GetUsers();
        public Task<User> GetUserById(int id);
        public Task<int> CreateUser(User user);

        public Task<User> UpdateUser(int id, User user);
    }
}
