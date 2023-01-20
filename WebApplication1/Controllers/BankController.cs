using System.Security.Claims;
using Bank.Models;
using Bank.Models.ViewModel;
using Bank.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
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

        private readonly ILogger _logger;

        public BankController(IBankService bankService, IHttpContextAccessor httpContextAccessor, IEmployeeService employeeService, ILogger<BankController> logger)
        {
            _bankService = bankService;
            _httpContextAccessor = httpContextAccessor;
            _employeeService = employeeService;
            _logger = logger;

            _logger.LogInformation("Inside the Bank Controller");
        }

        [HttpGet]
        public async Task<ActionResult<List<BankDetail>>> GetAllBanks()
        {
            var res = _bankService.GetBanks();

            _logger.LogInformation("GET Request : To get all banks", 10);

            return Ok(res);
        }

        [HttpPost, Authorize(Roles = "SuperAdmin")]
        public async Task<ActionResult<string>> CreateBank([FromBody] CreateBank createBankDetails)
        {

           

            string employeeEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            _logger.LogInformation("POST Request : Create a new Bank by {employeeEmail}", employeeEmail);

            Console.WriteLine(employeeEmail);

            bool isBankAlreadyExists = _employeeService.ValidateBank(createBankDetails.BankCode);

            string bankCodeToBeCreated = createBankDetails.BankCode;

            if(isBankAlreadyExists)
            {
                _logger.LogError("Bank with Code {bankCodeToBeCreated} already exists. So cannot create a new one with same code.", bankCodeToBeCreated);
                return BadRequest($"Bank Already Exists with Bank Code => {createBankDetails.BankCode}!!!");
            }

            var res = _bankService.CreateBank(createBankDetails, employeeEmail);

            _logger.LogInformation("Successfully Created a Bank with Code : {bankCodeToBeCreated} and created by {employeeEmail}", bankCodeToBeCreated, employeeEmail);

            return Ok(res);
        }

       
    }
}
