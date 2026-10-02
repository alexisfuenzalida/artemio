namespace MinimalAPIArtemio.Models
{
    public class ContractDetailReportModel
    {
        public string CostItem { get; set; } = "";
        public decimal Amount { get; set; }
        public string Include { get; set; } = "";
        public int ContractId { get; set; }
    }
}
