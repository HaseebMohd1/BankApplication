﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Models
{
    public class UserTransfer
    {

        public int Amount { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string? TransactionMethod { get; set; }

        

    }
}
