using Bank.Data.Contracts;
using Bank.LoggerService;
using Bank.Models;
using Bank.Models.ViewModel;
using Bank.Service.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace Bank.Service.Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly ILog _logger;

        public BankService(IBankRepository bankRepository, ILog logger)
        {
            _bankRepository = bankRepository;
            _logger = logger;
        }


        public List<BankDetail> GetBanks()
        {
            try
            {
                _logger.Information("Inside Bank.Servie to Fetch list of All Banks");
                var res = _bankRepository.GetBankDetails();
                if(res== null)
                {
                    _logger.Warning("List of Hospitals couldn't be fetched");
                }
                _logger.Information($"List of Banks Successfully fetched from Bank.Data Layer and sent to Bank Controller");
                return res;

            }
            catch (Exception ex)
            {
                _logger.Error("Something went wrong while fetching List of Banks in the Bank.Service");
                throw;
            }
            

        }

        public BankDetail CreateBank(CreateBank createBankDetails, string employeeName)
        {
            string bankCode = createBankDetails.BankCode;
            string bankName = createBankDetails.BankName;

            if(bankCode.IsNullOrEmpty() || bankName.IsNullOrEmpty())
            {
                throw new Exception("Please enter valid Names");
            }

            // Here, have to verify if the BankCode alreay exists or not before moving ahead
            


            BankDetail newBankDetails =  _bankRepository.CreateBank(bankCode, bankName, employeeName);

            return newBankDetails;

        }
    }
}
