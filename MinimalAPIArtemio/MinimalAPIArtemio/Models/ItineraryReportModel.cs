namespace MinimalAPIArtemio.Models
{
    public class ItineraryReportModel
    {
        public int ItineraryId { get; set; }
        public int Week { get; set; }
        public string Vessel { get; set; }
        public string Service { get; set; }
        public string Operator { get; set; }
        public string Companies { get; set; }
        public string POL {  get; set; }
        public string Stacking { get; set; }
        public string LateArrival { get; set; }
    }
}
