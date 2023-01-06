using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Models.AppDbContext;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {

        //private readonly  EmployeeDbContext _dbContext;

        // EmployeeDbContext db = new EmployeeDbContext();

        //public EmployeeController(EmployeeDbContext dbcontext)
        //{
        //    _dbContext = dbcontext;
        //}

        private IEmployeeRepository employeeRepository;

        

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
            
        }


        [Route("users")]
        [HttpGet]
        public Task<List<UserDto>> GetAllUsers() {

            //var response = await _dbContext.Employees.ToList();

            var response = employeeRepository.GetUsers();
            return response;
        }

        [Route("user/{id:int}")]
        [HttpGet]
        public Task<UserDto> GetUserById(int id)
        {
            var userDetails = employeeRepository.GetUserById(id);

            return userDetails;
        }

        [HttpPost]
        public async Task<User> CreateUser(User user)
        {
            int res = await  employeeRepository.CreateUser(user);

            if (res == 0)
            {
                return null;
            }

            return user;

        }

        
        [HttpPut]
        public async Task<UserDto> UpdateUser(int id, User user)
        {
            var res = await employeeRepository.UpdateUser(id, user);


            if(res == null)
            {
                return null;
            }

            return res;
        }


        [Route("employee/transaction/{id:int}")]
        [HttpPost]
        public async Task<Transaction> PerformTransaction(int id, Transaction transaction)
        {
            var res = await employeeRepository.performTransaction(id, transaction);
            return res;
        }
       




    }
}
