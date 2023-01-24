using System.Security.Claims;
using Bank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using WebApplication1.Services;
using Bank.Data.DTO;
using Bank.Models.ViewModel;
using Bank.Service.Contracts;

namespace WebApplication1.Controllers
{
    /// <summary>
    ///     
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        private IUserService _userService;


        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBankService _bankService;
        private readonly ILogger<UserController> _logger;

        public UserController(ITransactionService transactionService, IUserService userService, IHttpContextAccessor httpContextAccessor, IBankService bankService, ILogger<UserController> logger)
        {
            _transactionService = transactionService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _bankService = bankService;
            _logger = logger;
        }


        /// <summary>
        ///     <GET> Method : Returns the Transaction History of Logged in User
        ///     The User ID can be either in SenderId or ReceiverId
        /// </summary>
        /// <returns> List of All Transactions of that particular User </returns>
        [HttpGet("transactions"), Authorize(Roles = "user")]
        public List<Transaction> GetTransactionHistory()
        {
            var loggedInUserEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            _logger.LogInformation("Inside the GetTransactionHistory() controller by User => {UserEmail}" ,loggedInUserEmail);

            var loggedInUserId = _userService.GetUserIdByEmail(loggedInUserEmail);
            _logger.LogInformation("UserController : Retrieved User Id of {UserEmail}", loggedInUserEmail);

            var res = _transactionService.GetTransactionHistoryByUserId(loggedInUserId);

            _logger.LogInformation("GetTransactionHistory() Contoller ends here successfully => By User {UserEmail} ", loggedInUserEmail);

            return res;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount">  </param>
        /// <returns>  </returns>
        [HttpPost("withdrawal"), Authorize(Roles = "user")]
        public string WithdrawAmount([FromBody] int amount)
        {
            var loggedInUserEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var loggedInUserId = _userService.GetUserIdByEmail(loggedInUserEmail);

            string response = _userService.WithdrawAmount(amount, loggedInUserId);

            return response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userDepositDetails"></param>
        /// <returns></returns>
        [HttpPost("deposit"), Authorize(Roles ="user")]
        public async  Task<ActionResult<string>> DepositAmountByUser([FromBody] UserDeposit userDepositDetails)
        {

            int amount = userDepositDetails.Amount;

            string currency = userDepositDetails?.Currency;

            if ( !_bankService.isValidCurrency(currency))
            {
                return BadRequest("Please enter Valid Currency Code");
            }

            var loggedInUserEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var loggedInUserId = _userService.GetUserIdByEmail(loggedInUserEmail);

            string response = _userService.DepositAmount(amount, loggedInUserId, currency);

            return Ok(response);
        }




        [HttpPost("transfer"), Authorize(Roles = "user")]
        public Task<Transaction> TransferAmount(UserTransfer userTransferDetails)
        {
            var response = _userService.TransferAmount(userTransferDetails);
            return response;
        }




        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginModel userLoginDetails)
        {
            
            string userEmail = userLoginDetails.Email;
            string userPassword = userLoginDetails.Password;

            var res = _userService.UserLogin(userEmail, userPassword);

            if(res == null)
            {
                return BadRequest("Login Failed");
            }

            return Ok($"Login Successfull!!! => Token : {res}");
        }


        [HttpGet("logout"), Authorize(Roles = "user")]
        public async Task<ActionResult<string>> Logout()
        {
            var loggedInUserEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var loggedInUserId = _userService.GetUserIdByEmail(loggedInUserEmail);

            User userDetails = _userService.GetUserDetails(loggedInUserId);

            string tempRes = GetCurrentAsync();

            _httpContextAccessor.HttpContext.Request.Headers["authorization"] = string.Empty;

            var tempHttp = _httpContextAccessor.HttpContext.Items;

            return string.Empty;
        }





        private string GetCurrentAsync()
        {
            var authorizationHeader = HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }





        [HttpGet("details"), Authorize(Roles ="user")]
        public async Task<ActionResult<User>> UserDetails()
        {
            var loggedInUserEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var loggedInUserId = _userService.GetUserIdByEmail(loggedInUserEmail);

            User userDetails = _userService.GetUserDetails(loggedInUserId);


            return Ok(userDetails);
        }





        [HttpPost("resetpassword"), Authorize(Roles ="user")]
        public async Task<ActionResult<string>> ResetPassword(UserResetPassword userResetPassword)
        {

            if (!userResetPassword.NewPassword.Equals(userResetPassword.ConfirmPassword))
            {
                return BadRequest("New Password doesn't match with Confirm Password. Please try again!!");
            }

            var loggedInUserEmail = HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            bool res = _userService.ResetPassword(loggedInUserEmail, userResetPassword);

            if (!res)
            {
                return BadRequest("Password Reset Failed! Please try again!!");
            }

            return Ok("Password Updated Successfully!!");
        }
        

    }
}
