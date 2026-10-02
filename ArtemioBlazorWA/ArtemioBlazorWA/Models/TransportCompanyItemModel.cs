namespace ArtemioBlazorWA.Models
{
    public class TransportCompanyItemModel
    {
        public string Service { get; set; }
        public string Item { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public string Tax { get; set; }
        public string Concept { get; set; }
        public string Type { get; set; }
        public int TransportCompanyId { get; set; }
    }
}
