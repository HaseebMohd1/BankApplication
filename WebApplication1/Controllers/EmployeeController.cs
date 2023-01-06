using System.Collections;
using Microsoft.AspNetCore.Mvc;
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
        public Task<List<User>> GetAllUsers() {

            //var response = await _dbContext.Employees.ToList();

            var response = employeeRepository.GetUsers();
            return response;
        }

        [Route("user/{id:int}")]
        [HttpGet]
        public Task<User> GetUserById(int id)
        {
            var userDetails = employeeRepository.GetUserById(id);

            return userDetails;
        }

        [HttpPost]
        public async Task<string> CreateUser(User user)
        {
            int res = await employeeRepository.CreateUser(user);

            if (res == 0)
            {
                return "Error Occured While Creating User";
            }

            return "Successfully Added User!!!!";

        }
        


        //[HttpGet]
        //[Route("users")]
        //public  IEnumerable<User> GetUsers()
        //{
        //    var response =  _dbContext.Users ;
        //    return response;
        //}


        //[HttpPost]
        //[Route("user")]
        //public JsonContent CreateUser(User userDetails)
        //{
        //    _dbContext.Users.Add(new User()
        //    {
        //        UserName = userDetails.UserName,
        //        UserEmail = userDetails.UserEmail,
        //        UserPassword = userDetails.UserPassword,
        //        UserPhone = userDetails.UserPhone,
        //        BankCode = userDetails.BankCode,
        //        BankName = userDetails.BankName,
        //        AccountNumber = userDetails.AccountNumber,

        //    });
        //    var abcd =
        //    return abcd;
        //}


    }
}
