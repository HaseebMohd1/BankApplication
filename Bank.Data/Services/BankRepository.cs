using Bank.Data.Contracts;
using Bank.Models;
using SerilogTimings;
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
                //_logger.Information("Inside the BankRepository to fetch List of all Banks from `BankDetails` Table");
                List<BankDetail> bankDetails = _employeeDbContext.BankDetails.ToList();
                return bankDetails;
            }
            catch (Exception ex)
            {
                //_logger.Error("Error while fetching the list of all Banks form Database in the Data Layers(Repository)");
                throw;
            }
            
           
        }

        public BankDetail CreateBank(string bankCode, string bankName, string employeeName)
        {
            try
            {

                var newBank = new BankDetail()
                {
                    BankCode = bankCode,
                    BankName = bankName,
                    CreatedBy = employeeName,
                    CreatedOn = DateTime.Now
                };

                using (Operation.Time("Writing Bank Details in the Data Base"))
                {
                    var newCreatedBank = _employeeDbContext.BankDetails.Add(newBank);
                }

                    

                _employeeDbContext.SaveChanges();

                return newBank;
            }
            catch
            {
                throw new Exception("Error : Couldn't create a New Bank");
            }
        }


        public Currency GetCurrencyDetails(string currencyCode)
        {
            Currency currencyDetail = _employeeDbContext.Currencies.Where(c => c.CurrencyCode == currencyCode).FirstOrDefault();

            return currencyDetail;
        }
    }
}
