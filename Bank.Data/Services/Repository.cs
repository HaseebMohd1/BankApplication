using Bank.Models;
using Microsoft.Extensions.Logging;
using WebApplication1.Models.AppDbContext;

namespace WebApplication1.Repository
{
    public class Repository : IRepository
    {
        private readonly EmployeeDbContext _dbContext;
        private readonly ILogger<Repository> _logger;

        public Repository(EmployeeDbContext dbContext, ILogger<Repository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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



        /// <summary>
        ///     This method returns Transaction Details based on given User Id
        /// </summary>
        /// <param name="userId"> User Id </param>
        /// <returns> List of type Transaction.cs </returns>
        /// <exception cref="Exception"></exception>
        public List<Transaction> GetTransactionDetailsByUserId(int userId)
        {
            try
            {
                _logger.LogInformation("Repository : Inside Repository to fetch Transaction Details of User Id {userId}", userId);

                var userTransactions = _dbContext.Transactions.Where(t => t.SenderUserId == userId || t.ReceiverUserId == userId).ToList();

                _logger.LogInformation("Repository : Successfully retrieved Transaction Details of User Id {userId} from Transaction Table and returned...", userId);

                return userTransactions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Repository : Error while fetching User Transaction Details. Exception raised!!!");
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

        public bool UpdateUserDetails(User userDetails)
        {
            _dbContext.Users.Update(userDetails);
            _dbContext.SaveChanges();

            return true;
        }
    }
}
