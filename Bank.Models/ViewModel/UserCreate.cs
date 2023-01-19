using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Models.ViewModel
{
    public class UserCreate
    {

        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
        public string? Password { get; set; }
        public string? BankCode { get; set; }
        public string? BankName { get; set; }

        public int Amount { get; set; }

        
    }
}
