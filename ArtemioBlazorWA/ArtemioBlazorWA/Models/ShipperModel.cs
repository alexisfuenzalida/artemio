namespace ArtemioBlazorWA.Models
{
	public class ShipperModel
	{
		public int ShipperId { get; set; }
		public string Code { get; set; } = "";
		public string Shipper { get; set; } = "";
        public string Rut { get; set; } = "";
		public string Address { get; set; } = "";
		public int CityId { get; set; } = 1;
        public string Comment { get; set; } = "";
		public int Enabled { get; set; }
	}
}
