using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ItineraryDtlService
{
    private readonly ApiClient _api;

    public ItineraryDtlService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ItineraryDetailModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ItineraryDetailModel>("itinerariesdetails") ?? new List<ItineraryDetailModel>();
    }

    public async Task<List<ItineraryDetailModel>> GetAsync(int id)
    {
        return await _api.GetListAsync<ItineraryDetailModel>($"itinerariesdetails/{id}") ?? new List<ItineraryDetailModel>();
    }

    public async Task<ItineraryDetailModel> PostAsync(ItineraryDetailModel body)
    {
        return await _api.PostAsync<ItineraryDetailModel>("itinerariesdetails", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"itinerariesdetails/{id}");
    }

}
