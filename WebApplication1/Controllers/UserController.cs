using WebApplication1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        private IUserService _userService;

        public UserController(ITransactionService transactionService, IUserService userService)
        {
            _transactionService = transactionService;
            _userService = userService;
        }

       

        [HttpGet("transactions/{userId:int}")]
        public  List<Transaction> GetTransactionHistory(int userId)
        {
            var res =  _transactionService.GetTransactionHistoryByUserId(userId);
           
            return res;
        }

        [HttpPost("withdrawal/{userId:int}")]
        public string WithdrawAmount(int amount, int userId)
        {

            string response =  _userService.WithdrawAmount(amount, userId);

            return response;
        }

        [HttpPost("deposit")]
        public string DepositAmount(int amount, int userId)
        {

            string response = _userService.DepositAmount(amount, userId);

            return response;
        }


    }
}
