//using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Bank.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;
using WebApplication1.Models.AppDbContext;

//C: \Users\haseeb.m\source\repos\WebApplication1\Bank.Data\Models\AppDbContext\EmployeeDbContext.cs

namespace WebApplication1.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _dbContext;

        private readonly IMapper _mapper;

        private  IRepository _repository;

        public EmployeeRepository(EmployeeDbContext dbContext,  IMapper mapper, IRepository repository)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _repository = repository;

            
        }

        public async Task<List<UserDto>> GetUsers()
        {
            try
            {

               // var res = await _dbContext.Users.ToListAsync();

               // var res2 = await _dbContext.Users.Select(x => _mapper.Map<UserDto>(x)).ToListAsync();

                var res3 = await _dbContext.Users.Where(u => u.IsActive == 1).Select(x => _mapper.Map<UserDto>(x)).ToListAsync();

                //res.Select(u => _mapper.Map<UserDto>(u));

                //Console.WriteLine("res -> ", res);

                return res3;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public User GetUserDetails(int userId)
        {
            var userDetails = _dbContext.Users.Find(userId);

            return userDetails;
        }

        public async Task<UserDto> GetUserById(int id)
        {
            try
            {
                var res = await _dbContext.Users.FindAsync(id);

                if (res == null)
                {
                    throw new Exception("No Such User Exists!!");
                }

                var res2 = _mapper.Map<UserDto>(res);

                //var res2 = await _dbContext.Users.Select(x => _mapper.Map<UserDto>(x)).ToListAsync();

                

                return res2;

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> CreateUser(User user)
        {
            try
            {

                await _dbContext.Users.AddAsync(user);

 

                await _dbContext.SaveChangesAsync();

                return 1;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<UserDto> UpdateUser(int id, User user)
        {
            var result = await _dbContext.Users
                .FirstOrDefaultAsync(e => e.User_Id == id);

           

            if (result != null)
            {
                result.AccountNumber = user.AccountNumber;
                result.Role = user.Role;
                result.UserEmail = user.UserEmail;
                result.UserName = user.UserName;
                result.UserPhone = user.UserPhone;
                result.BankName = user.BankName;
                result.BankCode  = user.BankCode;
                result.Amount = user.Amount;
                result.UserPassword= user.UserPassword;


                await _dbContext.SaveChangesAsync();

                var result2 = _mapper.Map<UserDto>(result);

                return result2;
            }
            return null;
        }


        public Task<Transaction> performTransaction(int id, Transaction transaction)
        {

            try
            {
                int userBalance = _repository.UserBalance(id);

                double serviceCharge = 0;

                bool sameBank = false;

                User senderDetails = _repository.GetUserById(transaction.SenderUserId);
                User receiverDetails = _repository.GetUserById(transaction.ReceiverUserId);

                int minimumRequiredBalance = transaction.Amount;

                string senderBankCode = senderDetails?.BankCode;
                string receiverBankCode = receiverDetails?.BankCode;



                if (senderBankCode != null && receiverBankCode != null && senderBankCode == receiverBankCode)
                {
                    sameBank = true;
                }




                if (transaction.TransactionMethod != null && transaction.TransactionMethod.ToUpper() == "RTGS")
                {
                    if (sameBank)
                    {
                        minimumRequiredBalance = transaction.Amount;
                        serviceCharge = 0;
                    }
                    else
                    {
                        minimumRequiredBalance = transaction.Amount + (5 / 100) * transaction.Amount;
                        serviceCharge = (double)5 / 100 * (transaction.Amount);
                    }
                }


                if (transaction.TransactionMethod != null && transaction.TransactionMethod.ToUpper() == "IMPS")
                {
                    if (sameBank)
                    {
                        minimumRequiredBalance = transaction.Amount + (2 / 100) * transaction.Amount;
                        serviceCharge = (double)2 / 100 * (transaction.Amount);
                    }
                    else
                    {
                        minimumRequiredBalance = transaction.Amount + (6 / 100) * transaction.Amount;
                        serviceCharge = (double)6 / 100 * (transaction.Amount);
                    }
                }

                if (userBalance < minimumRequiredBalance)
                {
                    throw new Exception("Insufficient Amount!!");
                }

                transaction.DepositedAccount = minimumRequiredBalance;
                transaction.CreditedAccount = transaction.Amount;

                transaction.ServiceCharge = serviceCharge;





                _dbContext.Transactions.Add(transaction);
                senderDetails.Amount = senderDetails.Amount - minimumRequiredBalance;
                receiverDetails.Amount = receiverDetails.Amount + transaction.Amount;
                 _dbContext.SaveChanges();
                return Task.FromResult(transaction);
                }
            catch
            {
                throw new Exception("Transaction Failed!");
            }
            
        }


        public Transaction GetTransactionDetails(int transactionId)
        {

            try
            {
                var transactionDetails = _dbContext.Transactions.Find(transactionId);

                return transactionDetails;
            }
            catch
            {
                throw new Exception("Incorrect Transaction ID");
            }


            
            
        }

        public Task<Transaction> RevertTransaction(int transactionId)
        {
            Transaction transactionDetails = this.GetTransactionDetails(transactionId);
            try
            {


                if (transactionDetails == null)
                {
                    throw new Exception("Error : Incorrect Transaction Id");
                }

                User senderDetails = _repository.GetUserById(transactionDetails.SenderUserId);
                User receiverDetails = _repository.GetUserById(transactionDetails.ReceiverUserId);

                senderDetails.Amount = senderDetails.Amount + transactionDetails.DepositedAccount;
                receiverDetails.Amount = receiverDetails.Amount - transactionDetails.CreditedAccount;

                Transaction newTransaction = new Transaction()
                {
                    SenderUserId = transactionDetails.ReceiverUserId,
                    ReceiverUserId = transactionDetails.SenderUserId,
                    Amount = transactionDetails.Amount,
                    TransactionMethod = transactionDetails.TransactionMethod,
                    TransactionTime = new DateTime()
                };

                _dbContext.SaveChanges();

                var revertedTransaction = this.performTransaction(transactionDetails.ReceiverUserId, newTransaction);

                // _dbContext.Add()

                //_dbContext.SaveChanges();

                return Task.FromResult(transactionDetails);
            }
            catch
            {
                throw new Exception("Error while fetching Transaction Details!!");
            }


        }

        public List<Transaction> GetTransactionDetailsByUserId(int userId)
        {
            try
            {
                List<Transaction> userTransactionDetails = _repository.GetTransactionDetailsByUserId(userId);

                return userTransactionDetails;
            }
            catch
            {
                throw new Exception("Error : Couldn't get User Transaction Details!!");
            }
            
        }


        public string DeleteUserById(int id)
        {
            try
            {
                var userDetails = _dbContext.Users.Find(id);

                if (userDetails == null || userDetails?.IsActive==0)
                {
                    return "User Not Found!!!";
                }

                string? accountNumber = userDetails?.AccountNumber;
                string? userAccountNumber = accountNumber;

                userDetails.IsActive = 0;

                _dbContext.SaveChanges();

                return "User Removed Successfully -> Acc No : "+userAccountNumber;
            }
            catch
            {
                throw new Exception("Error Occured while deleting the User");
            }
            
        }

        public Employee GetEmployeeByEmail(string userEmail)
        {

            Employee employeeDetails = _dbContext.Employees.Where(e => e.EmployeeEmail.ToLower() == userEmail.ToLower()).FirstOrDefault();

            if(employeeDetails == null)
            {
                return null;
            }

            return employeeDetails;
        }

        public Employee SaveEmployee(Employee newEmployee)
        {
            _dbContext.Employees.Add(newEmployee);

            _dbContext.SaveChanges();

            var employeeDetails = _dbContext.Employees.Where(e => e.EmployeeEmail.ToLower() == newEmployee.EmployeeEmail.ToLower());

            if (employeeDetails == null)
            {
                throw new Exception("Employee Registration Failed!!!");
            }


            return newEmployee;
        }

    }
}
