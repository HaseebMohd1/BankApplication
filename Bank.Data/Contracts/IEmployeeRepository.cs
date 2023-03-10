using WebApplication1.DTO;
using Bank.Models;

namespace WebApplication1.Repository
{
    public interface IEmployeeRepository
    {
        public Task<List<UserDto>> GetUsers();
        public Task<UserDto> GetUserById(int id);

        public User GetUserDetails(int userId);
        public Task<int> CreateUser(User user);

        public Task<UserDto> UpdateUser(int id, User user);

        public Task<Transaction> performTransaction(int id, Transaction transaction);


        public Transaction GetTransactionDetails(int transactionId);

        public Task<Transaction> RevertTransaction(int transactionId, string employeeName);

        public List<Transaction> GetTransactionDetailsByUserId(int userId);

        public string DeleteUserById(int id);

        public Employee GetEmployeeByEmail(string userEmail);

        public Employee SaveEmployee(Employee newEmployee);

        //public User GetUserDetailsById(int userId);

        public Task<UserDto> UpdateUserNew(int id, User user);

        bool GetBank(string bankCode);

    }
}
