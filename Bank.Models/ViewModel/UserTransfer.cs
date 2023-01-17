namespace Bank.Models.ViewModel
{
    public class UserTransfer
    {

        public int Amount { get; set; }
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public string? TransactionMethod { get; set; }



    }
}
