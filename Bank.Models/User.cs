using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class User
    {
        [Key]
        public int User_Id { get; set; }

        public string? UserName { get; set; }

        public string? UserEmail { get; set;}

        public string? UserPhone { get; set;}

        public string? UserPassword { get; set;}

       // public string PasswordHash { get; set; } = string.Empty;

       //public string PasswordSalt { get; set; } = string.Empty;

        public string? BankCode { get; set; }

        public string? BankName { get; set;}

        public string? AccountNumber { get; set; }

        public int Amount { get; set; }
        public string? Role { get; set; } = "user";

        public int IsActive { get; set; } = 1;
    }
}
