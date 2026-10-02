namespace MinimalAPIArtemio.Models
{
    public class ContractDetailModel
    {
        public int CostItemId { get; set; }
        public decimal Amount { get; set; }
        public int PaymentConditionId { get; set; }
        public string Include { get; set; }
        public int ContractId { get; set; }
    }
}
