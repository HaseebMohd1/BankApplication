﻿using Bank.Models;

namespace WebApplication1.Services
{
    public interface ITransactionService
    {
        public List<Transaction> GetTransactionHistoryByUserId(int userId);
    }
}
