namespace ArtemioBlazorWA.Models
{
    public class PaymentModel
    {
        public int PaymentId { get; set; }
        public DateTime? Date { get; set; } = DateTime.Today;
        public decimal Amount { get; set; }
    }
}
