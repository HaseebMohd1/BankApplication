using Bank.Models;
using Bank.Models.ViewModel;
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

        public string CreateEmployee(string employeeName, string employeeEmail , string employeePassword);

        public Task<UserDto> UpdateUser(UserUpdate userRegisterDetails, string employeeEmail);


        public Task<Transaction> PerformTransaction(int id, TransactionModel transaction, string employeeEmail);

        public Task<Transaction> RevertTransaction(int id, string employeeEmail);

        bool ValidateBank(string bankCode);

        UserDto CreateUserNew(UserCreate userCreateDetails, string employeeName);

    }
}
