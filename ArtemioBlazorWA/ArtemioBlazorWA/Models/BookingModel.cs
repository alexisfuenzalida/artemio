namespace ArtemioBlazorWA.Models
{
    public class BookingModel
    {
        public int BookingId { get; set; }
        public DateTime? BookingDate { get; set; } = DateTime.Now;
        public DateTime? SendEmailDate { get; set; }
        public DateTime? UploadDate { get; set; }
        public string BookingStatus { get; set; } = "";
        public string ContractCo { get; set; } = "";
        public int TransportCompanyId { get; set; }
        public int AgentId { get; set; }
        public int AgentAduanaId { get; set; }
        public int AgentEmbarqueId { get; set; }
        public int IntermediaryId { get; set; }
        public int ContractTypeId { get; set; }
        public string PoRef { get; set; } = "";
        public int ShipperId { get; set; }
        public int ServiceId { get; set; }
        public int VesselId { get; set; }
        public string Voyage { get; set; } = "";
        public string NroRequest { get; set; } = "";
        public string NroBooking { get; set; } = "";
        public string AdelantoFolio { get; set; } = "";
        public int WarehouseId { get; set; }
        public DateTime? StackingFrom { get; set; }
        public DateTime? StackingUntil { get; set; }
        public DateTime? LAR { get; set; }
        public DateTime? LARDoc { get; set; }
        public DateTime? ExtraLAR { get; set; }
        public DateTime? CutOf { get; set; }
        public DateTime? LastETD { get; set; } = DateTime.Now;
        public DateTime? LastETA { get; set; }
        public int POLId { get; set; }
        public string Transport { get; set; } = "";
        public int LastPODId { get; set; }
        public string DeliveryPlace { get; set; } = "";
        public string ZipCode { get; set; } = "";
        public string Address { get; set; } = "";
        public int QtyContainers { get; set; }
        public int QtyContainersReal { get; set; }
        public int ContainerTypeId { get; set; }
        public bool ColdTreatment { get; set; } = false;
        public int CommodityId { get; set; }
        public string Temperature { get; set; } = "";
        public string Unit { get; set; } = "";
        public string CBM { get; set; } = "";
        public decimal PorcCBM { get; set; }
        public int PaymentConditionId { get; set; }
        public string StatusBL { get; set; } = "";
        public bool Roleo { get; set; } = false;
        public string RoleoBy { get; set; } = "";
        public bool TruckArtemio { get; set; } = false;
        public string TransportLand { get; set; } = "";
        public bool COD { get; set; } = false;
        public string DUS { get; set; } = "";
        public decimal Weight { get; set; }
        public int BlIssuedId { get; set; }
        public int IncotermId { get; set; }
        public int FreeDaysPOL { get; set; }
        public int FreeDaysPOD { get; set; }
        public decimal Net { get; set; }
        public decimal Public { get; set; }
        public decimal Rebate { get; set; }
        public string InvoiceLAR { get; set; } = "";
        public decimal AmountLAR { get; set; }
        public string InvoiceLARDoc { get; set; } = "";
        public decimal AmountLARDoc { get; set; }
        public string ReleaseBL { get; set; } = "";
        public string Status { get; set; } = "";
        public string Comments { get; set; } = "";
        public int SeasonId { get; set; }
        public DateTime? OriginalETD { get; set; } = DateTime.Now;
        public DateTime? OriginalETA { get; set; } = DateTime.Now;
        public int OriginalPODId { get; set; }
        public string Humidity { get; set; } = "";
        public int TransportId { get; set; }
        public string NroContract { get; set; } = "";
        public decimal FeeShipper { get; set; }
        public decimal FeeIntermediary { get; set; }
        public decimal FeeArtemio { get; set; }
        public decimal ArtemioCom { get; set; }
        public int ContractId { get; set; }
        public int TT {  get; set; }
        public string HsCode { get; set; } = "";
    }
}
