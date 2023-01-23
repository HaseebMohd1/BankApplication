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
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        private IUserService _userService;


        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBankService _bankService;

        public UserController(ITransactionService transactionService, IUserService userService, IHttpContextAccessor httpContextAccessor, IBankService bankService)
        {
            _transactionService = transactionService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _bankService = bankService;
        }



        [HttpGet("transactions"), Authorize(Roles = "user")]
        public List<Transaction> GetTransactionHistory()
        {
            var loggedInUserEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var loggedInUserId = _userService.GetUserIdByEmail(loggedInUserEmail);

            var res = _transactionService.GetTransactionHistoryByUserId(loggedInUserId);

            return res;
        }




        [HttpPost("withdrawal"), Authorize(Roles = "user")]
        public string WithdrawAmount([FromBody] int amount)
        {

            var loggedInUserEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var loggedInUserId = _userService.GetUserIdByEmail(loggedInUserEmail);

            string response = _userService.WithdrawAmount(amount, loggedInUserId);

            return response;
        }






        [HttpPost("deposit"), Authorize(Roles ="user")]
        public async  Task<ActionResult<string>> DepositAmountByUser([FromBody] UserDeposit userDepositDetails)
        {

            int amount = userDepositDetails.Amount;
            string currency = userDepositDetails?.Currency;

            if ( !_bankService.isValidCurrency(currency))
            {
                return BadRequest("Please enter Valid Currency Code");
            }

            var loggedInUserEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

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






        //[HttpPost("logintest")]
        //public async Task<ActionResult<string>> LoginTest(UserLogin userLoginDetails)
        //{
        //    string userEmail = userLoginDetails.userEmail;
        //    string userPassword = userLoginDetails.userPassword;

            

        //    var res = _userService.UserLogin(userEmail, userPassword);

        //    if (res == null)
        //    {
        //        return BadRequest("Login Failed");
        //    }

        //    return Ok($"Login Successfull!!! => Token : {res}");
            
        //}






        [HttpGet("logout"), Authorize(Roles = "user")]
        public async Task<ActionResult<string>> Logout()
        {

            var loggedInUserEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var loggedInUserId = _userService.GetUserIdByEmail(loggedInUserEmail);

            User userDetails = _userService.GetUserDetails(loggedInUserId);


            string tempRes = GetCurrentAsync();
            _httpContextAccessor.HttpContext.Request.Headers["authorization"] = string.Empty;

            var tempHttp = _httpContextAccessor.HttpContext.Items;

            //var testObj = new JwtSecurityTokenHandler();
            //var res = testObj.ReadToken("eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJkYXZpZEBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJ1c2VyIiwiZXhwIjoxNjczNTg3NTgxfQ.jsQYDj8kFBU8vQxUohcDoNGT8UBN8dFrRb6BsnZs0L0");


            return string.Empty;
        }





        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }





        [HttpGet("details"), Authorize(Roles ="user")]
        public async Task<ActionResult<User>> UserDetails()
        {
            var loggedInUserEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

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

            var loggedInUserEmail  = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);

            bool res = _userService.ResetPassword(loggedInUserEmail, userResetPassword);

            if (!res)
            {
                return BadRequest("Password Reset Failed! Please try again!!");
            }

            return Ok("Password Updated Successfully!!");
        }
        

    }
}
