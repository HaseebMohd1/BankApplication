using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface ITransactionRepository
    {
        public bool NewTransaction(Transaction transaction);

        public int GetCurrencyConversionValue(string currencyCode);
    }
}
