using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;

namespace MinimalAPIArtemio.Services;

public class ItineraryAppService
{
    public Task<List<ItineraryModel>> GetAllAsync()
    {
        var db = new ClaseDB<ItineraryModel>();
        return db.GetAllAsync("SELECT * FROM Itineraries;");
    }

    public Task<ItineraryModel?> GetAsync(int id)
    {
        var db = new ClaseDB<ItineraryModel>();
        return db.GetAsync("SELECT * FROM Itineraries WHERE ItineraryId = @id;", new { id });
    }
}