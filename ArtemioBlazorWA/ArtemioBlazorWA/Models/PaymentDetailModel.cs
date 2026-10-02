namespace ArtemioBlazorWA.Models
{
    public class PaymentDetailModel
    {
        public decimal Amount { get; set; }
        public int InvoiceId { get; set; }
        public int PaymentId { get; set; }
        public string InvoiceNumber { get; set; } = "";
    }
}
