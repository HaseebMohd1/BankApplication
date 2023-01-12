using Bank.Models;
using WebApplication1.Models.AppDbContext;

namespace WebApplication1.Repository
{
    public class Repository : IRepository
    {
        private readonly EmployeeDbContext _dbContext;

        public Repository(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public User GetUserById(int id)
        {
            try
            {
                var res = _dbContext.Users.Find(id);

                if (res == null)
                {
                    throw new Exception("No Such User Exists!!");
                }


                return res;

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int UserBalance(int userId)
        {
            var user = this.GetUserById(userId);

            if (user == null)
            {
                throw new Exception("User Not Found!!");
            }

            int userBalance = user.Amount;

            return userBalance;
        }
        public string UserBank(int userId)
        {
            User userDetails = this.GetUserById(userId);
            string userBankCode = userDetails.BankCode;
            return userBankCode;
        }


        public List<Transaction> GetTransactionDetailsByUserId(int userId)
        {
            try
            {
                var userTransactions = _dbContext.Transactions.Where(t => t.SenderUserId == userId || t.ReceiverUserId == userId).ToList();

                return userTransactions;
            }
            catch
            {
                throw new Exception("Error while fetching User Transaction Details!");
            }

        }

        public bool IsValidUser(int userId)
        {
            try
            {
                var userDetails = this.GetUserById(userId);
                if (userDetails != null && userDetails.IsActive == 1)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                throw new Exception("Error while validating a User!!");
            }

        }



        public User GetUserByEmail(string userEmail)
        {
            User userDetails = _dbContext.Users.Where(u => u.UserEmail.ToLower() == userEmail.ToLower()).FirstOrDefault();

            return userDetails;
        }


        public bool UpdateUserBalance(User userDetails)
        {
            _dbContext.Users.Update(userDetails);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
