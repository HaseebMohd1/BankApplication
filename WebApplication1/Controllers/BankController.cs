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
        private readonly ILogger<BankController> _logger;

        public BankController(IBankService bankService, IHttpContextAccessor httpContextAccessor, IEmployeeService employeeService, ILogger<BankController> logger)
        {
            _bankService = bankService;
            _httpContextAccessor = httpContextAccessor;
            _employeeService = employeeService;
            _logger = logger;

            //_logger.LogInformation("Inside the Bank Controller");
        }
        

        /// <summary>
        ///     
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllBanks()
        {
            var res = _bankService.GetBanks();
            
            return Ok(res);
        }



        [HttpPost, Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateBank([FromBody] CreateBank createBankDetails)
        {
            string employeeEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);

            string bankCodeToBeCreated = createBankDetails.BankCode;

            _logger.LogInformation("POST Request to Create a New Bank {BankCode} by {EmployeeName}", bankCodeToBeCreated, employeeEmail );

            Console.WriteLine(employeeEmail);

            bool isBankAlreadyExists = _employeeService.ValidateBank(createBankDetails.BankCode);

            if(isBankAlreadyExists)
            {
                //_logger.Error($"Bank with Code {bankCodeToBeCreated} already exists. So cannot create a new one with same code.");
                return BadRequest($"Bank Already Exists with Bank Code => {createBankDetails.BankCode}!!!");
            }

            var res = _bankService.CreateBank(createBankDetails, employeeEmail);

            //_logger.Information($"Successfully Created a Bank with Code : {bankCodeToBeCreated} and created by {employeeEmail}");

            return Ok(res);
        } 
    }
}
