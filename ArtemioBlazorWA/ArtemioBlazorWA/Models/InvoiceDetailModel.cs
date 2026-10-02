namespace ArtemioBlazorWA.Models
{
    public class InvoiceDetailModel
    {
        public decimal Amount { get; set; }
        public int BookingId { get; set; }
        public int InvoiceId { get; set; }
        public string NroBooking { get; set; } = "";
    }
}
