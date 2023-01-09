using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Transaction
    {
        private double serviceCharge;

        [Key]
        public int TransactionId { get; set; }

        public DateTime TransactionTime { get; set; }
        public int Amount { get; set; }

        public int SenderUserId { get; set; }

        public int ReceiverUserId { get; set; }

        public string? TransactionMethod { get; set; }

        public int DepositedAccount { get; set; }
        public int CreditedAccount { get; set; }

        public double ServiceCharge { get => serviceCharge; set => serviceCharge = value; }

        public string TransactionCurrency { get; set; } = "INR";



    }
}
