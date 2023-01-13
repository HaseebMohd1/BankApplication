using Bank.Models;
using WebApplication1.DTO;
using WebApplication1.Repository;
using BCrypt.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;
using Bank.Models.ViewModel;
using Azure.Core;

namespace WebApplication1.Services
{
    public class UserService : IUserService
    {


        private readonly ITransactionRepository _transactionRepository;
        private readonly IRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;

        private readonly IConfiguration _configuration;

        

        public UserService(ITransactionRepository transactionRepository, IRepository repository, IEmployeeRepository employeeRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _transactionRepository = transactionRepository;
            _repository = repository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _configuration = configuration;
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

            userDetails.Amount -= amount;

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

            _repository.UpdateUserBalance(userDetails);

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
                userDetails.Amount += amount;
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



        //public Task<Transaction> TransferAmount(Transaction transaction)
        //{

        //    var userId = transaction.SenderUserId;

        //    var res =  _employeeRepository.performTransaction(userId, transaction);


        //    return res;
        //}

        public Task<Transaction> TransferAmount(UserTransfer transaction)
        {

            var userId = transaction.SenderUserId;

            Transaction newTransaction = new Transaction
            {
                SenderUserId = transaction.SenderUserId,
                ReceiverUserId = transaction.ReceiverUserId,
                Amount = transaction.Amount,
                TransactionTime = DateTime.Now,
                TransactionMethod = transaction.TransactionMethod
                
                   
            };

            var res = _employeeRepository.performTransaction(userId, newTransaction);


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

            string token = CreateToken(userDetails);

            return token;
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

        private string GenerateRandomSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }



        private string CreateToken(User user)
        {

                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // definig PAYLOAD of our JSON Web Token    
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        public int GetUserIdByEmail(string userEmail)
        {
            User userDetails = _repository.GetUserByEmail(userEmail);
            if (userDetails == null)
            {
                throw new Exception("User Details doesn't exist!! -> Called from User Controller -> transactions");
            }

            int userId = userDetails.User_Id;
            return userId;
        }

        public User GetUserDetails(int userId)
        {
            User userDetails = _repository.GetUserById(userId);

            if(userDetails == null)
            {
                throw new Exception("Error occured : User Not Found => Trying to Logout");
            }

            return userDetails;
        }

        public bool ResetPassword(string userEmail, UserResetPassword userResetPassword)
        {

            //User userDetails = _repository.GetUserByEmail(userEmail) ?? new User();
            User userDetails = _repository.GetUserByEmail(userEmail);
            if (userDetails == null)
            {
                throw new Exception("Error occured : User Not Found => Trying to Reset Password");
            }

            string hashedPassword = userDetails.UserPassword;
            string passwordSalt = userDetails.PasswordSalt;

            if (!VerifyPasswordSha256(userResetPassword.OldPassword, hashedPassword, passwordSalt))
            {
                throw new Exception("Entered Details are Incorrect. Please Enter valid Email & Password");
            }



            

            // Password Hashing using SHA256
            string newRandomSalt = GenerateRandomSalt();

            string newHashedPassword = CreatePasswordHashUsingSha256(userResetPassword.NewPassword + newRandomSalt);


            userDetails.UserPassword = newHashedPassword;
            userDetails.PasswordSalt = newRandomSalt;

            bool isUpdateSuccessful =  _repository.UpdateUserDetails(userDetails);

            return isUpdateSuccessful;
        }



    }
}
