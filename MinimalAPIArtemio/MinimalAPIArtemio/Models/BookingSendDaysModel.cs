namespace MinimalAPIArtemio.Models
{
    public class BookingSendDaysModel
    {
        public string Country { get; set; } = "";
        public string Vessel { get; set; } = "";
        public string Voyage { get; set; } = "";
        public string NroBooking { get; set; } = "";
        public string POD { get; set; } = "";
        public string PoRef { get; set; } = "";
        public string BL { get; set; } = "";
        public string Container { get; set; } = "";
        public string Shipper { get; set; } = "";
        public string FreeDaysComment { get; set; } = "";
        public DateTime ETD { get; set; }
    }
}
