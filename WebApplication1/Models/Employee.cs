﻿using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        public string? EmployeeName { get; set; }

        [Required]
        public string? EmployeeEmail { get; set; }

        [Required]
        public string? Role { get; set; } = "Admin";

    }
}
