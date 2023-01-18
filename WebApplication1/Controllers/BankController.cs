using System.Security.Claims;
using Bank.Models;
using Bank.Models.ViewModel;
using Bank.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace Bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IEmployeeService _employeeService;

      
        public BankController(IBankService bankService, IHttpContextAccessor httpContextAccessor, IEmployeeService employeeService)
        {
            _bankService = bankService;
            _httpContextAccessor = httpContextAccessor;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<BankDetail>>> GetAllBanks()
        {
            var res = _bankService.GetBanks();
            return Ok(res);
        }

        [HttpPost, Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<string>> CreateBank([FromBody] CreateBank createBankDetails)
        {

            string employeeEmail = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            Console.WriteLine(employeeEmail);

            bool isBankAlreadyExists = _employeeService.ValidateBank(createBankDetails.BankCode);

            if(isBankAlreadyExists)
            {
                return BadRequest($"Bank Already Exists with Bank Code => {createBankDetails.BankCode}!!!");
            }

            var res = _bankService.CreateBank(createBankDetails, employeeEmail);

            return Ok(res);
        }

       
    }
}
