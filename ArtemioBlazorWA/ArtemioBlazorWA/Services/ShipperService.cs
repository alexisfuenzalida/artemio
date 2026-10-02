using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ShipperService
{
    private readonly ApiClient _api;

    public ShipperService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ShipperModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ShipperModel>("shippers") ?? new List<ShipperModel>();
    }

    public async Task<ShipperModel> GetAsync(int id)
    {
        return await _api.GetAsync<ShipperModel>($"shippers/{id}");
    }

    public async Task<ShipperModel> PostAsync(ShipperModel body)
    {
        return await _api.PostAsync<ShipperModel>("shippers", body);
    }

    public async Task<ShipperModel> PutAsync(ShipperModel body)
    {
        return await _api.PutAsync<ShipperModel>("shippers", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"shippers/{id}");
    }

}
