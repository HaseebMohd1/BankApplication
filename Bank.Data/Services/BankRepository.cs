using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bank.Data.Contracts;
using Bank.Models;
using WebApplication1.Models.AppDbContext;

namespace Bank.Data.Services
{
    public class BankRepository : IBankRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;
        public BankRepository(EmployeeDbContext employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

        

        public List<BankDetail> GetBankDetails()
        {
            try
            {
                List<BankDetail> bankDetails = _employeeDbContext.BankDetails.ToList();
                return bankDetails;
            }
            catch
            {
                throw new Exception("Error occured while fetching data of all banks!");
            }
           
        }

        public BankDetail CreateBank(string bankCode, string bankName)
        {
            try
            {

                var newBank = new BankDetail()
                {
                    BankCode = bankCode,
                    BankName = bankName
                };

                var newCreatedBank = _employeeDbContext.BankDetails.Add(newBank);

                _employeeDbContext.SaveChanges();

                return newBank;
            }
            catch
            {
                throw new Exception("Error : Couldn't create a New Bank");
            }
        }
    }
}
