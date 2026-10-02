namespace ArtemioBlazorWA.Models
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }
        public DateTime? Date { get; set; } = DateTime.Today;
        public string Type { get; set; } = "";
        public string Number { get; set; } = "";
        public decimal Amount { get; set; }
        public string Observation { get; set; } = "";
    }
}
