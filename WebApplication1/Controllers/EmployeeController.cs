using Bank.Models;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Repository;
using WebApplication1.Services;
using Bank.Models;


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

        public EmployeeController(IEmployeeRepository employeeRepository, IEmployeeService employeeService)
        {
            this.employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }


        [Route("users")]
        [HttpGet]
        public Task<List<UserDto>> GetAllUsers()
        {

            //var response = await _dbContext.Employees.ToList();

            //var response = employeeRepository.GetUsers();

            var response = _employeeService.GetUsers();

            return response;
        }

        [Route("user/{id:int}")]
        [HttpGet]
        public UserDto GetUserById(int id)
        {
            //var userDetails = employeeRepository.GetUserById(id);

            var userDetails2 = _employeeService.GetUserById(id);

            return userDetails2;
        }

        [HttpPost]
        public async Task<User> CreateUser(User user)
        {
            int res = await employeeRepository.CreateUser(user);

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


            if (res == null)
            {
                return null;
            }

            return res;
        }


        [Route("transaction/{id:int}")]
        [HttpPost]
        public async Task<Transaction> PerformTransaction(int id, Transaction transaction)
        {
            var res = await employeeRepository.performTransaction(id, transaction);
            return res;
        }

        [Route("revert/{transactionId:int}")]
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


        [Route("transactionDetails/{userId:int}")]
        [HttpGet]
        public async Task<List<Transaction>> GetUserTransactionDetails(int userId)
        {
            var res = employeeRepository.GetTransactionDetailsByUserId(userId);
            return res;
        }


        [HttpPut("user/delete/{userId:int}")]
        public async Task<string> DeleteUser(int userId)
        {
            var res = employeeRepository?.DeleteUserById(userId);
            return res;
        }







    }
}
