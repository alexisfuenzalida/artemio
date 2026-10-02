namespace ArtemioBlazorWA.Models
{
    public class BookingHsCodeGrid
    {
        public int ShipperId { get; set; }
        public int CommodityId { get; set; }
        public string Shipper { get; set; } = "";
        public string Commodity { get; set; } = "";
        public string HsCode { get; set; } = "";
    }
}
