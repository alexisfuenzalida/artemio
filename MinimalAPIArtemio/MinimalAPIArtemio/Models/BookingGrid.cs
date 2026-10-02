namespace MinimalAPIArtemio.Models
{
    public class BookingGrid
    {
        public int BookingId { get; set; }
        public int ETDWeek { get; set; }
        public string NroBooking { get; set; }
        public string NroRequest { get; set; }
        public string PO { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string BookingStatus { get; set; }
        public string ShippingCompany { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public string Shipper { get; set; }
        public string Commodity { get; set; }
        public int QtyContainers { get; set; }
        public string BL { get; set; }
        public string Container { get; set; }
        public string Status { get; set; }
    }
}
