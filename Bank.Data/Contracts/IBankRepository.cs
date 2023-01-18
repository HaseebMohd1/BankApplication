using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Models;

namespace Bank.Data.Contracts
{
    public interface IBankRepository
    {
        BankDetail CreateBank(string bankCode, string bankName, string employeeName);
        public List<BankDetail> GetBankDetails();
    }
}
