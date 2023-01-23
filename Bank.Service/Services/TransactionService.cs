using Bank.Models;
using Microsoft.Extensions.Logging;
using WebApplication1.Repository;

namespace WebApplication1.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IRepository _repository;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ITransactionRepository transactionRepository, IRepository repository, ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _repository = repository;
            _logger = logger;
        }


        /// <summary>
        ///     This method returns the Transaction History of a User based on the given input Id
        /// </summary>
        /// <param name="userId"> LoggedIn Users' Unique Id </param>
        /// <returns> List of Transaction </returns>
        /// <exception cref="Exception"></exception>
        public List<Transaction> GetTransactionHistoryByUserId(int userId)
        {
            try
            {
                _logger.LogInformation("Transaction Service : Inside GetTransactionHistoryByUserId() Method to fetch details of Id {userId}", userId);

                if (!_repository.IsValidUser(userId))
                {
                    _logger.LogError("Transaction Service : User with Id {userid} couldn't be found. Exceptin Raised!!", userId);

                    throw new Exception("Error while fetching User Transaction Details : User is Invalid");
                }

                _logger.LogInformation("Transaction Service : User {userId} is successfully verfied", userId);

                List<Transaction> transactionHistory = _repository.GetTransactionDetailsByUserId(userId);

                _logger.LogInformation("Transaction Service : Successfully fetched Transaction Details of User with Id {userid} and returning...", userId);

                return transactionHistory;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction Service : User with Id {userid} couldn't be found. Exceptin Raised!!", userId);
                throw ex;
            }
            
        }

    }
}