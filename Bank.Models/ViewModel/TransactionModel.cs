using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Models.ViewModel
{
    public class TransactionModel
    {
        public int Amount { get; set; }

        public int SenderUserId { get; set; }

        public int ReceiverUserId { get; set; }

        public string? TransactionMethod { get; set; }

    }
}
