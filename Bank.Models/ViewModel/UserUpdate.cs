using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Models.ViewModel
{
    public class UserUpdate
    {
        public int UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserPhone { get; set; }
    }
}
