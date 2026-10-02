namespace MinimalAPIArtemio.Models
{
    public class PreInvoiceModel
    {
        public int PreInvoiceId { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public int BookingId { get; set; }
        public string NroBooking { get; set; } = "";
    }
}
