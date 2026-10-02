namespace MinimalAPIArtemio.Models
{
    public class ContractReportModel
    {
        public int ContractId { get; set; }
        public string TransportCompany { get; set; } = "";
        public string NroContract { get; set; } = "";
        public DateTime From { get; set; }
        public DateTime Until { get; set; }
        public string Commodity { get; set; } = "";
        public string POL { get; set; } = "";
        public string POT1 { get; set; } = "";
        public string POT2 { get; set; } = "";
        public string POT3 { get; set; } = "";
        public string POD { get; set; } = "";
        public string Country { get; set; } = "";
        public int TT { get; set; }
        public string FreeDaysOrigin { get; set; } = "";
        public string FreeDaysDestination { get; set; } = "";
        public decimal Public { get; set; }
        public decimal VolumeDiscount { get; set; }
        public decimal Net { get; set; }
        public decimal FreeArtemio { get; set; }
        public decimal TotalNotInclude { get; set; }
        public string Service { get; set; } = "";
        public string Comment { get; set; } = "";
        public string Shipper { get; set; } = "";
        public string Status { get; set; } = "";
    }
}
