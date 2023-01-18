﻿using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class BankDetail
    {
        [Key]
        public int BankId { get; set; }

        [Required]
        public string? BankName { get; set; }

        [Required]
        public string? BankCode { get; set; }
    }
}
