namespace MinimalAPIArtemio.Models
{
    public class StackingModel
    {
        public int StackingId { get; set; }
        public int Week {  get; set; }
        public int ServiceId { get; set; }
        public string Stacking { get; set; }
        public string LateArrival { get; set; }
        public string ExtraLateArrival { get; set; }
        public string CutOff { get; set; }
    }
}
