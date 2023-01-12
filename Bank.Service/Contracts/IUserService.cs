using Bank.Models;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public interface IUserService
    {
        public string WithdrawAmount(int amount, int userId);

        public string DepositAmount(int amount, int userId, string currency);

        public Task<Transaction> TransferAmount(UserTransfer transaction);

        public string UserLogin(string userName, string userPassword);

        public int GetUserIdByEmail(string userEmail);

        public User GetUserDetails(int userId);

       // public Task<Transaction> TransferAmountTest(UserTransfer transaction);

    }
}
