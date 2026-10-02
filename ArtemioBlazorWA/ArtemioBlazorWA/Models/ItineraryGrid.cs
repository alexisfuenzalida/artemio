namespace ArtemioBlazorWA.Models
{
    public class ItineraryGrid
    {
        public int ItineraryId { get; set; }
        public int Week { get; set; }
        public DateTime? ETD { get; set; }
        public string TransportCompany { get; set; } = "";
        public string Service {  get; set; } = "";
        public string Vessel { get; set; } = "";
        public string POL { get; set; } = "";
    }
}
