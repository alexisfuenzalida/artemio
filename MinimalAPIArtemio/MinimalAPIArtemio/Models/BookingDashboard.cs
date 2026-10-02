namespace MinimalAPIArtemio.Models
{
    public class BookingDashboard
    {
        public int BookingId { get; set; }
        public DateTime LastETD { get; set; }
        public DateTime LastETA { get; set; }
        public string ContractType { get; set; }
        public int WK { get; set; }
        public string StatusBL { get; set; }
        public string PoRef { get; set; }
        public string Shipper { get; set; }
        public string NroRequest { get; set; }
        public string NroBooking { get; set; }
        public int QtyContainers { get; set; }
        public int QtyContainersReal { get; set; }
    }
}
