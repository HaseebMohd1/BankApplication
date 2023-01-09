using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {


        private readonly ITransactionRepository _transactionRepository;
        private readonly IRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;

        public UserService(ITransactionRepository transactionRepository, IRepository repository, IEmployeeRepository employeeRepository)
        {
            _transactionRepository = transactionRepository;
            _repository = repository;
            _employeeRepository = employeeRepository;
        }

        private bool CheckIfSufficientBalance(int amount, int userId)
        {
            var userDetails = _repository.GetUserById(userId);

            double userAmount = userDetails.Amount;

            if( userAmount < amount)
            {
                return false;
            }


            return true;
        }

        public string WithdrawAmount(int amount, int userId)
        {
            var userDetails =  _repository.GetUserById(userId);

            double userAmount = userDetails.Amount;

            if(userAmount < amount)
            {
                throw new Exception("Insufficient Balance");
            }

            Transaction newTransaction = new Transaction
            {
                Amount = amount,
                TransactionMethod = "WITHDRAWAL",
                ServiceCharge = 0,
                SenderUserId = userId,
                ReceiverUserId = -1,
                CreditedAccount = amount,
                DepositedAccount = -1,
                TransactionTime = DateTime.UtcNow,
            };

            bool transactionSuccessfull = _transactionRepository.NewTransaction(newTransaction);

            if(!transactionSuccessfull)
            {
                throw new Exception("Withdrawal Failed");
            }

            var remainingBalance = _repository.GetUserById(userId).Amount;

            return "Withdrawal Successfull : -> Remaining Balance : "+remainingBalance;
        }



        public string DepositAmount(int amount, int userId, string currency)
        {
            var userDetails = _repository.GetUserById(userId);

            if(currency.ToUpper() != "INR")
            {
                userDetails.Amount += amount * 90;
            }
            else
            {
                userDetails.Amount = amount;
            }
            //userDetails.Amount += amount;


            Transaction newTransaction = new Transaction
            {
                Amount = amount,
                TransactionMethod = "DEPOSIT",
                ServiceCharge = 0,
                SenderUserId = userId,
                ReceiverUserId = -1,
                CreditedAccount = amount,
                DepositedAccount = -1,
                TransactionTime = DateTime.UtcNow,
                TransactionCurrency = currency
                
            };

            bool isTransactionSuccessfull = _transactionRepository.NewTransaction(newTransaction);

            if(!isTransactionSuccessfull)
            {
                return "Error Occured : Couldn't Deposit";
            }

            var updatedBalance = _repository.GetUserById(userId).Amount;

            return "Deposit Successfull : -> Updated Balance : " + updatedBalance;
        }



        public Task<Transaction> TransferAmount(Transaction transaction)
        {

            var userId = transaction.SenderUserId;

            var res =  _employeeRepository.performTransaction(userId, transaction);


            return res;
        }

        
    }
}
