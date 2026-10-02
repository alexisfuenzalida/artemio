namespace MinimalAPIArtemio.Models
{
    public class ItineraryETDModel
    {
        public int POLId { get; set; }
        public DateTime ETD { get; set; }
        public bool IsMain { get; set; }
        public int ItineraryId { get; set; }
    }
}
