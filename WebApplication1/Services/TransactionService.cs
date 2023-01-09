using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IRepository _repository;
        public TransactionService(ITransactionRepository transactionRepository, IRepository repository)
        {
            _transactionRepository = transactionRepository;
            _repository = repository;
        }



        public List<Transaction> GetTransactionHistoryByUserId(int userId)
        {

            if (!_repository.IsValidUser(userId))
            {
                throw new Exception("Error while fetching User Transaction Details");
            }
            List<Transaction> transactionHistory = _repository.GetTransactionDetailsByUserId(userId);

            return transactionHistory;
        }

    }
}
