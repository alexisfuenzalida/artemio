namespace MinimalAPIArtemio.Models
{
    public class BookingStackingGrid
    {
        public DateTime LastETD { get; set; }
        public int VesselId { get; set; }
        public string Vessel { get; set; } = "";
        public string Voyage { get; set; } = "";
        public int TransportCompanyId { get; set; }
        public string TransportCompany { get; set; } = "";
        public int POLId { get; set; }
        public string POL { get; set; } = "";
        public string PoRef { get; set; } = "";
    }
}
