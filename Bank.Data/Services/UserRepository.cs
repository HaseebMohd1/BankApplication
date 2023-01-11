using Bank.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.AppDbContext;

namespace WebApplication1.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;

        public UserRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        public string GetHashedPassword(string userEmail)
        {
            var hashedPassword = _employeeDbContext.Users.FromSql($"SELECT [UserPassword] FROM Users WHERE UserEmail = '{userEmail}'").ToList();
            return hashedPassword.ElementAt(0).UserPassword;
        }
    }
}
