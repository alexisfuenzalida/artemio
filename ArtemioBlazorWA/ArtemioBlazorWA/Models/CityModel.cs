using System.ComponentModel.DataAnnotations;

namespace ArtemioBlazorWA.Models
{
	public class CityModel
	{
		public int CityId { get; set; }
		public string City { get; set; } = "";
		public int CountryId { get; set; }
	}
}
