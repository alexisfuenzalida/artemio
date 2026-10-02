using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class PortService
{
    private readonly ApiClient _api;

    public PortService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<PortModel>> GetAllAsync()
    {
        return await _api.GetListAsync<PortModel>("ports") ?? new List<PortModel>();
    }

    public async Task<PortModel> GetAsync(int id)
    {
        return await _api.GetAsync<PortModel>($"ports/{id}");
    }

    public async Task<PortModel> PostAsync(PortModel body)
    {
        return await _api.PostAsync<PortModel>("ports", body);
    }

    public async Task<PortModel> PutAsync(PortModel body)
    {
        return await _api.PutAsync<PortModel>("ports", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"ports/{id}");
    }

}
