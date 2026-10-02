namespace ArtemioBlazorWA.Models
{
    public class BookingIncidenceGrid
    {
        public int BookingId { get; set; }
        public string NroBooking { get; set; } = "";
        public string NroRequest { get; set; } = "";
        public string BL { get; set; } = "";
        public string Container { get; set; } = "";
        public string Consignee { get; set; } = "";
        public string Notify { get; set; } = "";
        public int Incidences { get; set; }
    }
}
