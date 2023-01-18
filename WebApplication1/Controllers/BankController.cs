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

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
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
            var res = _bankService.CreateBank(createBankDetails);

            return Ok(res);
        }

       
    }
}
