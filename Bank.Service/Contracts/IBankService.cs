using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Models;
using Bank.Models.ViewModel;

namespace Bank.Service.Contracts
{
    public interface IBankService
    {
        public List<BankDetail> GetBanks();

        BankDetail CreateBank(CreateBank createBankDetails, string employeeName);

        bool isValidCurrency(string currencyCode);
    }
}
