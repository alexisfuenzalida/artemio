namespace ArtemioBlazorWA.Models
{
    public class TtModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int POLId { get; set; }
        public DateTime? _From { get; set; }
        public DateTime? _To { get; set; }        
    }
}
