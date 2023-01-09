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
        public UserController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        

        [HttpGet("transactions/{userId:int}")]
        public  List<Transaction> GetTransactionHistory(int userId)
        {
            var res =  _transactionService.GetTransactionHistoryByUserId(userId);
           
            return res;
        }
    }
}
