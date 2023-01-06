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

        public EmployeeRepository(EmployeeDbContext dbContext,  IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            try
            {

                var res = await _dbContext.Users.ToListAsync();

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

        public async Task<User> GetUserById(int id)
        {
            try
            {
                var res = await _dbContext.Users.FindAsync(id);

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

        public async Task<int> CreateUser(User user)
        {
            try
            {
               // Guid obj = new Guid();

                //  var res = await _dbContext.Users.FindAsync(user.UserEmail);

                //if (res != null)
                //{
                //  return 0;
                // }

               // var res = from u in _dbContext.Users where u.UserEmail == user.UserEmail select u;

               // if(res != null)
               // {
                //    return 0;
                //}

                await _dbContext.Users.AddAsync(user);

 

                await _dbContext.SaveChangesAsync();

                return 1;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<User> UpdateUser(int id, User user)
        {
            var result = await _dbContext.Users
                .FirstOrDefaultAsync(e => e.User_Id == id);


            if(result != null)
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

                return result;
            }
            return null;
        }
    }
}
