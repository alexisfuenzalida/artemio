namespace MinimalAPIArtemio.Models
{
    public class ItineraryReportETAModel
    {
        public string POD { get; set; }
        public DateTime? ETA { get; set; }
        public int ItineraryId { get; set; }
    }
}
