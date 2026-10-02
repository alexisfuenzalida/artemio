namespace MinimalAPIArtemio.Models
{
    public class ContractGrid
    {
        public int ContractId { get; set; }
        public string TransportCompany {  get; set; } = "";
        public string NroContract { get; set; } = "";
        public DateTime From { get; set; }
        public DateTime Until { get; set; }
        public string Commodity { get; set; } = "";
        public string POL {  get; set; } = "";
        public string POD { get; set; } = "";
        public int TT { get; set; }
        public decimal Public {  get; set; }
        public decimal Net { get; set; }
        public decimal VD { get; set; }
        public decimal Sum { get; set; }
        public string RateType { get; set; } = "";

    }
}
