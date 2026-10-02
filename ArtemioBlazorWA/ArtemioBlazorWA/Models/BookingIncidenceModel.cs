namespace ArtemioBlazorWA.Models
{
    public class BookingIncidenceModel
    {
        public string Container { get; set; } = "";
        public int IncidenceTypeId { get; set; }
        public DateTime Date { get; set; }
        public string Incidence { get; set; } = "";
        public int BookingId { get; set; }
    }
}
