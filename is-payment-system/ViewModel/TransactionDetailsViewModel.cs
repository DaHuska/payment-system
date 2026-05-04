namespace is_payment_system.ViewModel
{
    public class TransactionDetailsViewModel
    {
        public string Recipient { get; set; }
        public string Description { get; set; }
        public string TransactionIdText { get; set; }
        public string Date { get; set; }
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string Subtotal { get; set; }
        public string Fee { get; set; }
        public string Total { get; set; }
    }
}