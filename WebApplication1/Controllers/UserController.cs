using System.Security.Claims;
using Bank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        private IUserService _userService;

        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserController(ITransactionService transactionService, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _transactionService = transactionService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }



        //[HttpGet("transactions/{userId:int}"), Authorize(Roles ="user")]
        //public List<Transaction> GetTransactionHistory(int userId)
        [HttpGet("transactions"), Authorize(Roles = "user")]
        public List<Transaction> GetTransactionHistory()
        {
            //var temp = User?.Identity.Name;

            var loggedInUserEmail =_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            var loggedInUserId = _userService.GetUserIdByEmail(loggedInUserEmail);

            var res = _transactionService.GetTransactionHistoryByUserId(loggedInUserId);

            return res;
        }

        [HttpPost("withdrawal/{userId:int}"), Authorize(Roles = "user")]
        public string WithdrawAmount(int amount, int userId)
        {
            
            string response = _userService.WithdrawAmount(amount, userId);

            return response;
        }

        [HttpPost("deposit")]
        public string DepositAmount(int amount, int userId, string currency)
        {

            string response = _userService.DepositAmount(amount, userId, currency);

            return response;
        }

        [HttpPost("transfer")]
        public Task<Transaction> TransferAmount(Transaction transaction)
        {
            var response = _userService.TransferAmount(transaction);
            return response;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(string userEmail, string userPassword)
        {
            //if (user.UserName != userDetails.UserName)
            //{
            //    return BadRequest("User Not Found");
            //}

            //if (!VerifyPassword(userDetails.Password, user.PasswordSalt, user.PasswordHash))
            //{
            //    return BadRequest("Incorrect Credentials!!!");
            //}

            //string token = CreateToken(user);

            var res = _userService.UserLogin(userEmail, userPassword);

            if(res == null)
            {
                return BadRequest("Login Failed");
            }

            return Ok($"Login Successfull!!! => Token : {res}");
        }

    }
}
