namespace MinimalAPIArtemio.Models
{
    public class ItineraryFilterModel
    {
        public int POLId { get; set; }
        public int TransportCompanyId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int ServiceId { get; set; }
        public int SeasonId { get; set; }
        public int[] PODId { get; set; }
    }
}
