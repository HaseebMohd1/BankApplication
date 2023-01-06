//using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Models.AppDbContext;

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

                var res2 = await _dbContext.Users.Select(x => _mapper.Map<UserDto>(x)).ToListAsync();
                //res.Select(u => _mapper.Map<UserDto>(u));

                //Console.WriteLine("res -> ", res);

                return res2;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

                

                bool sameBank = false;

                User senderDetails = _repository.GetUserById(transaction.SenderUserId);
                User receiverDetails = _repository.GetUserById(transaction.ReceiverUserId);

                int minimumRequiredBalance = senderDetails.Amount;

                string senderBankCode = senderDetails.BankCode;
                string receiverBankCode = receiverDetails.BankCode;

                if (senderBankCode != null && receiverBankCode != null && senderBankCode == receiverBankCode)
                {
                    sameBank = true;
                }


                

                if ( transaction.TransactionMethod !=  null && transaction.TransactionMethod.ToUpper() == "RTGS")
                {
                    if (sameBank)
                    {
                        minimumRequiredBalance = transaction.Amount;
                    }
                    else
                    {
                        minimumRequiredBalance = transaction.Amount + (5/100) * transaction.Amount;
                    }
                }


                if(transaction.TransactionMethod != null && transaction.TransactionMethod.ToUpper() == "IMPS")
                {
                    if(sameBank)
                    {
                        minimumRequiredBalance = transaction.Amount + (2 / 100) * transaction.Amount;
                    }
                    else
                    {
                        minimumRequiredBalance = transaction.Amount + (6 / 100) * transaction.Amount;
                    }
                }

                if (userBalance < minimumRequiredBalance)
                {
                    throw new Exception("Insufficient Amount!!");
                }
                

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
    }
}
