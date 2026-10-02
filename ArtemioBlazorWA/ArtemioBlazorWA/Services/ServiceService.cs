using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ServiceService
{
    private readonly ApiClient _api;

    public ServiceService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ServiceModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ServiceModel>("services") ?? new List<ServiceModel>();
    }

    public async Task<ServiceModel> GetAsync(int id)
    {
        return await _api.GetAsync<ServiceModel>($"services/{id}");
    }

    public async Task<ServiceModel> PostAsync(ServiceModel body)
    {
        return await _api.PostAsync<ServiceModel>("services", body);
    }

    public async Task<ServiceModel> PutAsync(ServiceModel body)
    {
        return await _api.PutAsync<ServiceModel>("services", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"services/{id}");
    }

}
