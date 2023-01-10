using Bank.Models;
using WebApplication1.Models.AppDbContext;

namespace WebApplication1.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly EmployeeDbContext _dbContext;
        public TransactionRepository(EmployeeDbContext employeeDbContext)
        {
            _dbContext= employeeDbContext;
        }

        public bool NewTransaction(Transaction transaction)
        {
            try
            {
                _dbContext.Add(transaction);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public int GetCurrencyConversionValue(string currencyCode)
        {

            var currecnyDetails = _dbContext.Currencies.Find(currencyCode);

            if(currecnyDetails == null)
            {
                return 0;
            }

            return currecnyDetails.ConversionValue;
        }


    }
}
