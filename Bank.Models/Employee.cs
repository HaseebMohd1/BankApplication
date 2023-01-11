using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string? EmployeeName { get; set; }

        [Required]
        public string? EmployeeEmail { get; set; }

        
        public string? Role { get; set; } = "Admin";

        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;

    }
}
