namespace ArtemioBlazorWA.Models
{
    public class ItineraryModel
    {
        public int ItineraryId { get; set; }
        public int Week { get; set; }
        public DateTime? ETD { get; set; }
        public int POLId { get; set; }
        public int VesselId { get; set; }
        public int ServiceId { get; set; }
        public int OperatorId { get; set; }
        public int SeasonId { get; set; }
        public string Voyage { get; set; } = "";
        public string Stacking { get; set; }
        public string LateArrival { get; set; }
        public string ExtraLateArrival { get; set; }
        public string CutOff { get; set; }
    }
}
