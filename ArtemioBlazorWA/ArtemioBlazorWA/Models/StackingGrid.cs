namespace ArtemioBlazorWA.Models
{
    public class StackingGrid
    {
        public int StackingId { get; set; }
        public int Week { get; set; }
        public string Service { get; set; }
        public string Stacking { get; set; }
        public string LateArrival { get; set; }
        public string ExtraLateArrival { get; set; }
        public string CutOff { get; set; }
    }
}
