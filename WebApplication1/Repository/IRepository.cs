using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IRepository
    {
        public int UserBalance(int userId);

        public User GetUserById(int id);
        public string UserBank(int userId);
       
    }
}
