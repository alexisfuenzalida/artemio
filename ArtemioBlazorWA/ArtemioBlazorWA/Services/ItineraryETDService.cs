using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ItineraryETDService
{
    private readonly ApiClient _api;

    public ItineraryETDService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ItineraryETDModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ItineraryETDModel>("itinerariesetd") ?? new List<ItineraryETDModel>();
    }

    public async Task<List<ItineraryETDModel>> GetAsync(int id)
    {
        return await _api.GetListAsync<ItineraryETDModel>($"itinerariesetd/{id}") ?? new List<ItineraryETDModel>();
    }

    public async Task<ItineraryETDModel> PostAsync(ItineraryETDModel body)
    {
        return await _api.PostAsync<ItineraryETDModel>("itinerariesetd", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"itinerariesetd/{id}");
    }

}
