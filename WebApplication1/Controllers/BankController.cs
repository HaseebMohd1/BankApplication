using System.Security.Claims;
using Bank.Models;
using Bank.Models.ViewModel;
using Bank.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public BankController(IBankService bankService, IHttpContextAccessor httpContextAccessor)
        {
            _bankService = bankService;
            _httpContextAccessor = httpContextAccessor; 
        }

        [HttpGet]
        public async Task<ActionResult<List<BankDetail>>> GetAllBanks()
        {
            var res = _bankService.GetBanks();
            return Ok(res);
        }

        [HttpPost, Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<BankDetail>> CreateBank([FromBody] CreateBank createBankDetails)
        {

            string employeeEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            Console.WriteLine(employeeEmail);

            var res = _bankService.CreateBank(createBankDetails, employeeEmail);

            return Ok(res);
        }

       
    }
}
