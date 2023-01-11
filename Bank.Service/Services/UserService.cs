using Bank.Models;
using WebApplication1.DTO;
using WebApplication1.Repository;
using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {


        private readonly ITransactionRepository _transactionRepository;
        private readonly IRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;

        public UserService(ITransactionRepository transactionRepository, IRepository repository, IEmployeeRepository employeeRepository, IUserRepository userRepository)
        {
            _transactionRepository = transactionRepository;
            _repository = repository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
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

            int currencyConverterValue = _transactionRepository.GetCurrencyConversionValue(currency);
            if(currency.ToUpper() != "INR")
            {
                userDetails.Amount += amount * currencyConverterValue;
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

        public string UserLogin(string userEmail, string userPassword)
        {

            User userDetails = _repository.GetUserByEmail(userEmail);

            if(userDetails == null)
            {
                throw new Exception("User with Email doesn't exists!!! Please enter valid/existing User Email");
            }


            // string hashedPassword = _userRepository.GetHashedPassword(userEmail);

            string hashedPassword = userDetails.UserPassword;
            string passwordSalt = userDetails.PasswordSalt;

            //if(!VerifyPassword(userPassword, hashedPassword, passwordSalt))
            //{
            //    throw new Exception("Entered Details are Incorrect. Please Enter valid Email & Password");
            //}

            if (!VerifyPasswordSha256(userPassword, hashedPassword, passwordSalt))
            {
                throw new Exception("Entered Details are Incorrect. Please Enter valid Email & Password");
            }


            return "Login";
        }


        private bool VerifyPassword(string password, string passwordSalt, string passwordHash)
        {
            using (var hmac = new HMACSHA512(Encoding.ASCII.GetBytes(passwordSalt)))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                bool isPasswordMatching = computedHash.SequenceEqual(Encoding.ASCII.GetBytes(passwordHash));

                return isPasswordMatching ;
            }
        }

        private bool VerifyPasswordSha256(string password, string hashedPassword, string salt)
        {
            string userEnteredHashedPassword = CreatePasswordHashUsingSha256(password + salt);

            return userEnteredHashedPassword.Equals(hashedPassword);
        }

        private string CreatePasswordHashUsingSha256(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var resBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Console.WriteLine(resBytes + "---");

                var hashedPassword = BitConverter.ToString(resBytes).Replace("-", "").ToLower();

                //Console.WriteLine(resString + "\n");

                return hashedPassword;

            }
        }

    }
}
