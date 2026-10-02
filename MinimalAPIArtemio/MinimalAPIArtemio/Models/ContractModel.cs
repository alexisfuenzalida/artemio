namespace MinimalAPIArtemio.Models
{
    public class ContractModel
    {
        public int ContractId { get; set; }
        public int TransportCompanyId { get; set; }
        public string NroContract { get; set; } = "";
        public DateTime From { get; set; }
        public DateTime Until { get; set; }
        public int CommodityId { get; set; }
        public int POLId { get; set; }
        public string POT1 { get; set; } = "";
        public string POT2 { get; set; } = "";
        public string POT3 { get; set; } = "";
        public int PODId { get; set; }
        public int TT {  get; set; }
        public string FreeDaysOrigin { get; set; } = "";
        public string FreeDaysDestination { get; set; } = "";
        public decimal Public {  get; set; }
        public decimal VolumeDiscount { get; set; }
        public decimal Net {  get; set; }
        public decimal FreeArtemio { get; set; }
        public int ServiceId { get; set; }
        public string Comment { get; set; } = "";
        public int SeasonId { get; set; }
        public int ShipperId { get; set; }
        public string Status { get; set; } = "";
        public string RateType { get; set; } = "";
    }
}
