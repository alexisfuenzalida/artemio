namespace ArtemioBlazorWA.Models
{
    public class BookingContractGridModel
    {
        public string NroContract { get; set; } = "";
        public string Shipper { get; set; } = "";
        public string Intermediary { get; set; } = "";
        public decimal Public { get; set; } = 0;
        public decimal Net { get; set; } = 0;
        public decimal FeeShipper { get; set; } = 0;
        public decimal FeeIntermediary { get; set; } = 0;
        public decimal Comm { get; set; } = 0;
        public int ContractId { get; set; } = 0;
    }
}
