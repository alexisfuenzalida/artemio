namespace ArtemioBlazorWA.Models
{
    public class ContractConditionModel
    {
        public int ShipperId { get; set; }
        public int IntermediaryId { get; set; }
        public decimal Public { get; set; }
        public decimal Net { get; set; }
        public decimal FeeShipper { get; set; }
        public decimal FeeIntermediary { get; set; }
        public decimal FeeArtemio { get; set; }
        public decimal Comm { get; set; }
        public int ContractId { get; set; }
    }
}
