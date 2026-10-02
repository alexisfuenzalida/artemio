using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class IncotermService
{
    private readonly ApiClient _api;

    public IncotermService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<IncotermModel>> GetAllAsync()
    {
        return await _api.GetListAsync<IncotermModel>("incoterms") ?? new List<IncotermModel>();
    }

    public async Task<IncotermModel> GetAsync(int id)
    {
        return await _api.GetAsync<IncotermModel>($"incoterms/{id}");
    }

    public async Task<IncotermModel> PostAsync(IncotermModel body)
    {
        return await _api.PostAsync<IncotermModel>("incoterms", body);
    }

    public async Task<IncotermModel> PutAsync(IncotermModel body)
    {
        return await _api.PutAsync<IncotermModel>("incoterms", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"incoterms/{id}");
    }

}
