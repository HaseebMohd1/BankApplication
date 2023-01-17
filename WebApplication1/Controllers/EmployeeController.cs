﻿using System.Security.Claims;
using Bank.Models;
using Bank.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Repository;
using WebApplication1.Services;


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

        private readonly IEmployeeService _employeeService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeController(IEmployeeRepository employeeRepository, IEmployeeService employeeService, IHttpContextAccessor httpContextAccessor)
        {
            this.employeeRepository = employeeRepository;
            _employeeService = employeeService;
            _httpContextAccessor = httpContextAccessor;
        }


        [Route("users"), Authorize(Roles ="Admin,SuperAdmin")]
        [HttpGet]
        public Task<List<UserDto>> GetAllUsers()
        {

            

            //var response = await _dbContext.Employees.ToList();

            //var response = employeeRepository.GetUsers();

            var response = _employeeService.GetUsers();

            return response;
        }

        [Route("user/{id:int}"), Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public UserDto GetUserById(int id)
        {
            //var userDetails = employeeRepository.GetUserById(id);

            var userDetails2 = _employeeService.GetUserById(id);

            return userDetails2;
        }

        [HttpPost, Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<UserDto> CreateUser(User user)
        {
            // int res = await employeeRepository.CreateUser(user);

            string employeeEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            user.CreatedBy = employeeEmail;
            user.CreatedOn = DateTime.UtcNow;

            UserDto newUserDetails = _employeeService.CreateUser(user);

            //if (res == 0)
            //{
            //    return null;
            //}

            return newUserDetails;

          // return user;

        }

        


        [HttpPut, Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<UserDto> UpdateUser(int id, User user)
        {


            string employeeEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            user.ModifiedBy = employeeEmail;
            user.ModifiedOn = DateTime.UtcNow;

            var res = await employeeRepository.UpdateUser(id, user);


            if (res == null)
            {
                return null;
            }

            return res;
        }


        [Route("transaction/{id:int}"), Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<Transaction> PerformTransaction(int id, Transaction transaction)
        {
            var res = await employeeRepository.performTransaction(id, transaction);
            return res;
        }

        [Route("revert/{transactionId:int}"), Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<Transaction> RevertTransaction(int transactionId)
        {
            Transaction revertedTransaction = await employeeRepository.RevertTransaction(transactionId);

            if (revertedTransaction == null)
            {
                throw new Exception("Something went wrong while perfoming Reverted Transaction");
            }

            return revertedTransaction;
        }


        [Route("transactionDetails/{userId:int}"), Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<List<Transaction>> GetUserTransactionDetails(int userId)
        {
            var res = employeeRepository.GetTransactionDetailsByUserId(userId);
            return res;
        }


        [HttpPut("user/delete/{userId:int}"), Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<string> DeleteUser(int userId)
        {
            var res = employeeRepository?.DeleteUserById(userId);
            return res;
        }

        [HttpPost("/login")]
        public async Task<ActionResult<string>> EmployeeLogin(string employeeEmail, string password)
        {
            string res =  _employeeService.EmployeeLogin(employeeEmail, password);

            if(res == null || res == string.Empty)
            {
                return BadRequest("Employee Login Failed!!");
            } 

            return Ok(res);
        }

        [HttpPost("/registerEmployee"), Authorize(Roles ="SuperAdmin")]
        public async Task<ActionResult<string>> RegisterEmployee(string empName, string empEmail, string empPassword)
        {
            var res = _employeeService.CreateEmployee(empName, empEmail, empPassword);

            if (res == string.Empty)
            {
                return BadRequest();
            }

            return Ok(res);
        }

    }
}
