using Bank.Data.Contracts;
using Bank.Models;
using Bank.Models.ViewModel;
using Bank.Service.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace Bank.Service.Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;

        public BankService(IBankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }


        public List<BankDetail> GetBanks()
        {
            var res = _bankRepository.GetBankDetails();

            return res;

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
