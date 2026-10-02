namespace ArtemioBlazorWA.Models
{
    public class BookingDetailModel
    {
        public string BL { get; set; } = "";
        public string Container { get; set; } = "";
        public int ConsigneeId { get; set; }
        public int NotifyId { get; set; }
        public int Cases { get; set; }
        public int StatusContainerId { get; set; }
        public string Comment { get; set; } = "";
        public int BookingId { get; set; }
        public decimal Comm { get; set; }
        public decimal Rebate { get; set; }
        public decimal Fee { get; set; }
        public decimal RebateExp { get; set; }
        public bool SendFreeDays { get; set; }
        public string SendFreeDaysComment { get; set; } = "";
    }
}
