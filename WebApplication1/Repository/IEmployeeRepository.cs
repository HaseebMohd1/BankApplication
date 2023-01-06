using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IEmployeeRepository
    {
        public Task<List<UserDto>> GetUsers();
        public Task<UserDto> GetUserById(int id);
        public Task<int> CreateUser(User user);

        public Task<UserDto> UpdateUser(int id, User user);

        public Task<Transaction> performTransaction(int id, Transaction transaction);


        public Transaction GetTransactionDetails(int transactionId);

        public Task<Transaction> RevertTransaction(int transactionId);

        public List<Transaction> GetTransactionDetailsByUserId(int userId);
    }
}
