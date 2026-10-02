using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class VesselService
{
    private readonly ApiClient _api;

    public VesselService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<VesselModel>> GetAllAsync()
    {
        return await _api.GetListAsync<VesselModel>("vessels") ?? new List<VesselModel>();
    }

    public async Task<VesselModel> GetAsync(int id)
    {
        return await _api.GetAsync<VesselModel>($"vessels/{id}");
    }

    public async Task<VesselModel> PostAsync(VesselModel body)
    {
        return await _api.PostAsync<VesselModel>("vessels", body);
    }

    public async Task<VesselModel> PutAsync(VesselModel body)
    {
        return await _api.PutAsync<VesselModel>("vessels", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"vessels/{id}");
    }

}
