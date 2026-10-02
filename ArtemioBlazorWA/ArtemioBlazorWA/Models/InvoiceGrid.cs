namespace ArtemioBlazorWA.Models
{
    public class InvoiceGrid
    {
        public int InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";
        public string Number { get; set; } = "";
        public decimal Amount { get; set; }
        public string Observation { get; set; } = "";
        public string Bookings { get; set; } = "";
    }
}
