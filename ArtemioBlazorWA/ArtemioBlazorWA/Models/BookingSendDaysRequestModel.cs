namespace ArtemioBlazorWA.Models
{
    public class BookingSendDaysItemModel
    {
        public int BookingId { get; set; }
        public string BL { get; set; } = "";
    }

    public class BookingSendDaysRequestModel
    {
        public List<string> BL { get; set; } = new List<string>();
        public List<int> BookingIds { get; set; } = new List<int>();
        public List<BookingSendDaysItemModel> Items { get; set; } = new List<BookingSendDaysItemModel>();
        public string Email { get; set; } = "";
        public string Country { get; set; } = "";
        public int CountryId { get; set; }
    }
}
