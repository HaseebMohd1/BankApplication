using Bank.Models;

namespace WebApplication1.Repository
{
    public interface IRepository
    {
        public int UserBalance(int userId);

        public User GetUserById(int id);
        public string UserBank(int userId);

        public List<Transaction> GetTransactionDetailsByUserId(int userId);

        public bool IsValidUser(int userId);

        public User GetUserByEmail(string userEmail);


    }
}
