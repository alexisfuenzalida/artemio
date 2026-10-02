namespace MinimalAPIArtemio.Models
{
    public class BookingSendModel
    {
        public int BookingId { get; set; }
        public int ETDWeek { get; set; }
        public DateTime? ETD { get; set; }
        public string ShippingCo {  get; set; }
        public string Shipper { get; set; }
        public string PoRef { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string NroBooking { get; set; }
        public string POL {  get; set; }
        public string POD { get; set; }
        public string DeliveryPlace { get; set; }
        public string PaymentCondition { get; set; }
        public string Temperature { get; set; } = "";
        public string CBM { get; set; }
        public string Commodity { get; set; }
        public int QtyContainers { get; set; }
        public string NroRequest { get; set; }
        public string Container { get; set; }
        public string BL { get; set; }
    }
}
