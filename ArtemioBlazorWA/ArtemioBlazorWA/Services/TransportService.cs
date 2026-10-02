using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class TransportService
{
    private readonly ApiClient _api;

    public TransportService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<TransportModel>> GetAllAsync()
    {
        return await _api.GetListAsync<TransportModel>("transports") ?? new List<TransportModel>();
    }

    public async Task<TransportModel> GetAsync(int id)
    {
        return await _api.GetAsync<TransportModel>($"transports/{id}");
    }

    public async Task<TransportModel> PostAsync(TransportModel body)
    {
        return await _api.PostAsync<TransportModel>("transports", body);
    }

    public async Task<TransportModel> PutAsync(TransportModel body)
    {
        return await _api.PutAsync<TransportModel>("transports", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"transports/{id}");
    }

}
