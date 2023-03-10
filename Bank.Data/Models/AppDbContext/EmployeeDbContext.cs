using Microsoft.EntityFrameworkCore;
using Bank.Models;


namespace WebApplication1.Models.AppDbContext
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set;}
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Currency> Currencies { get; set; }

        public DbSet<BankDetail> BankDetails { get; set; }
    }
}
