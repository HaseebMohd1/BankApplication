using Bank.Models;

namespace WebApplication1.Repository
{
    public interface IUserRepository
    {
        public string GetHashedPassword(string userEmail);
    }
}
