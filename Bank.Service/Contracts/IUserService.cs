using Bank.Models;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public interface IUserService
    {
        public string WithdrawAmount(int amount, int userId);

        public string DepositAmount(int amount, int userId, string currency);

        public Task<Transaction> TransferAmount(Transaction transaction);

        public string UserLogin(string userName, string userPassword);
       
    }
}
