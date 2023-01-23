using System.Security.Claims;
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
            var response = _employeeService.GetUsers();

            return response;
        }



        [Route("user/{id:int}"), Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public UserDto GetUserById(int id)
        {
            var userDetails2 = _employeeService.GetUserById(id);

            return userDetails2;
        }




        [HttpPost("createUser"), Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<ActionResult<string>> CreateUserNew(UserCreate userCreateDetails)
        {

            string employeeEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);


            bool bankExists = _employeeService.ValidateBank(userCreateDetails.BankCode);

            if (!bankExists)
            {
                return BadRequest("This Bank doesn't exits. Please enter valid Bank Code");
            }

            UserDto newUserDetails = _employeeService.CreateUserNew(userCreateDetails, employeeEmail);


            string successMessage = $"Name : {newUserDetails.UserName}\n\tEmail : {newUserDetails.UserEmail}\n\tAccount Number : {newUserDetails.AccountNumber}";

            return Ok($"User Succesfully Created : -> \n\t {successMessage}");

        }



    
        [HttpPut("updateUser"), Authorize(Roles = "Admin,SuperAdmin")]
        public async  Task<ActionResult<UserDto>> UpdateUserTest(UserUpdate userUpdateDetails)
        {
            string employeeEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var res = await _employeeService.UpdateUser(userUpdateDetails, employeeEmail);

            if (res == null)
            {
                return null;
            }

            return Ok(res);
        }




        [Route("transaction"), Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<Transaction> PerformTransaction([FromBody] TransactionModel transaction)
        {
            int id = transaction.SenderUserId;

            string employeeEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var res2 = await _employeeService.PerformTransaction(id, transaction, employeeEmail);

            return res2;
        }

        [Route("revert/{transactionId:int}"), Authorize(Roles = "Admin,SuperAdmin")]
        [HttpGet]
        public async Task<Transaction> RevertTransaction(int transactionId)
        {
            string employeeEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            Transaction revertedTransaction = await _employeeService.RevertTransaction(transactionId, employeeEmail);

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
        public async Task<ActionResult<string>> EmployeeLogin(LoginModel loginDetails)
        {
            string employeeEmail = loginDetails.Email;
            string password = loginDetails.Password;

            string res =  _employeeService.EmployeeLogin(employeeEmail, password);

            if(res == null || res == string.Empty)
            {
                return BadRequest("Employee Login Failed!!");
            } 

            return Ok(res);
        }



        // <summary>
        // This end point creates a New Employee
        // This can be accessed only by Employess with Role='SuperAdmin'    
        // </summary>
        [HttpPost("/registerEmployee"), Authorize(Roles ="SuperAdmin")]
        public async Task<ActionResult<string>> RegisterEmployee(RegisterEmployeeModel employeeDetails)
        {
            string empName = employeeDetails.EmployeeName;
            string empEmail = employeeDetails.EmployeeEmail;
            string empPassword = employeeDetails.EmployeePassword;

            var res = _employeeService.CreateEmployee(empName, empEmail, empPassword);

            if (res == string.Empty)
            {
                return BadRequest();
            }

            return Ok(res);
        }

    }
}
