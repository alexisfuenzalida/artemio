namespace ArtemioBlazorWA.Models
{
    public class InvoiceBalanceModel
    {
        public int InvoiceId { get; set; }
        public string Number { get; set; } = "";
        public string Type { get; set; } = "";
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Balance => Amount - AmountPaid;
    }
}
