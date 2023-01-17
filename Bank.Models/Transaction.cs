using System.ComponentModel.DataAnnotations;

namespace Bank.Models
{
    public class Transaction
    {
        private double serviceCharge;

        [Key]
        public int TransactionId { get; set; }

        public DateTime TransactionTime { get; set; } =  DateTime.Now;
        public int Amount { get; set; }

        public int SenderUserId { get; set; }

        public int ReceiverUserId { get; set; }

        public string? TransactionMethod { get; set; }

        public int DepositedAccount { get; set; }
        public int CreditedAccount { get; set; }

        public double ServiceCharge { get => serviceCharge; set => serviceCharge = value; }

        public string TransactionCurrency { get; set; } = "INR";

        public string TransactionUniqueId { get; set; } = string.Empty;

        public string CreatedBy { get; set; } = string.Empty;


    }
}
