using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class StatusContainerService
{
    private readonly ApiClient _api;

    public StatusContainerService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<StatusContainerModel>> GetAllAsync()
    {
        return await _api.GetListAsync<StatusContainerModel>("statuscontainers") ?? new List<StatusContainerModel>();
    }

    public async Task<StatusContainerModel> GetAsync(int id)
    {
        return await _api.GetAsync<StatusContainerModel>($"statuscontainers/{id}");
    }

    public async Task<StatusContainerModel> PostAsync(StatusContainerModel body)
    {
        return await _api.PostAsync<StatusContainerModel>("statuscontainers", body);
    }

    public async Task<StatusContainerModel> PutAsync(StatusContainerModel body)
    {
        return await _api.PutAsync<StatusContainerModel>("statuscontainers", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"statuscontainers/{id}");
    }

}
