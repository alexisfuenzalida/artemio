using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class BlIssuedService
{
    private readonly ApiClient _api;

    public BlIssuedService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<BlIssuedModel>> GetAllAsync()
    {
        return await _api.GetListAsync<BlIssuedModel>("blissueds") ?? new List<BlIssuedModel>();
    }

    public async Task<BlIssuedModel> GetAsync(int id)
    {
        return await _api.GetAsync<BlIssuedModel>($"blissueds/{id}");
    }

    public async Task<BlIssuedModel> PostAsync(BlIssuedModel body)
    {
        return await _api.PostAsync<BlIssuedModel>("blissueds", body);
    }

    public async Task<BlIssuedModel> PutAsync(BlIssuedModel body)
    {
        return await _api.PutAsync<BlIssuedModel>("blissueds", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"blissueds/{id}");
    }

}
