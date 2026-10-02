namespace MinimalAPIArtemio.Models
{
    public class PaymentGrid
    {
        public int PaymentId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Invoices { get; set; } = "";
    }
}
