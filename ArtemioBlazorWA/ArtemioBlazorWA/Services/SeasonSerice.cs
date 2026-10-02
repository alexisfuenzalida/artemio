using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class SeasonSerice
{
    private readonly ApiClient _api;

    public SeasonSerice(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<SeasonModel>> GetAllAsync()
    {
        return await _api.GetListAsync<SeasonModel>("seasons") ?? new List<SeasonModel>();
    }

    public async Task<SeasonModel> GetAsync(int id)
    {
        return await _api.GetAsync<SeasonModel>($"seasons/{id}");
    }

    public async Task<SeasonModel> PostAsync(SeasonModel body)
    {
        return await _api.PostAsync<SeasonModel>("seasons", body);
    }

    public async Task<SeasonModel> PutAsync(SeasonModel body)
    {
        return await _api.PutAsync<SeasonModel>("seasons", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"seasons/{id}");
    }

}
