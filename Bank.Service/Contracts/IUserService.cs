using Bank.Models;

namespace WebApplication1.Services
{
    public interface IUserService
    {
        public string WithdrawAmount(int amount, int userId);

        public string DepositAmount(int amount, int userId, string currency);

        public Task<Transaction> TransferAmount(Transaction transaction);

    }
}
