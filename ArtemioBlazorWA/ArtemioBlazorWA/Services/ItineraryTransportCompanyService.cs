using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ItineraryTransportCompanyService
{
    private readonly ApiClient _api;

    public ItineraryTransportCompanyService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ItineraryTransportCompanyModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ItineraryTransportCompanyModel>("itinerariestransportcompanies") ?? new List<ItineraryTransportCompanyModel>();
    }

    public async Task<List<ItineraryTransportCompanyModel>> GetAsync(int id)
    {
        return await _api.GetListAsync<ItineraryTransportCompanyModel>($"itinerariestransportcompanies/{id}") ?? new List<ItineraryTransportCompanyModel>();
    }

    public async Task<ItineraryTransportCompanyModel> PostAsync(ItineraryTransportCompanyModel body)
    {
        return await _api.PostAsync<ItineraryTransportCompanyModel>("itinerariestransportcompanies", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"itinerariestransportcompanies/{id}");
    }

}
