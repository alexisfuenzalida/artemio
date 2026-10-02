namespace ArtemioBlazorWA.Models
{
    public class ItineraryImportResultModel
    {
        public int TotalRows { get; set; }
        public int Imported { get; set; }
        public int Skipped { get; set; }
        public List<ItineraryImportRowError> Errors { get; set; } = new();
    }

    public class ItineraryImportRowError
    {
        public int Row { get; set; }
        public string Message { get; set; } = "";
    }
}
