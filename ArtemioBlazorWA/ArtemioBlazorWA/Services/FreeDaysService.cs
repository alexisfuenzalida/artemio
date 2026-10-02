using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class FreeDaysService
{
    private readonly ApiClient _api;

    public FreeDaysService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<FreeDaysModel>> GetAllAsync()
    {
        return await _api.GetListAsync<FreeDaysModel>("freedays") ?? new List<FreeDaysModel>();
    }

    public async Task<FreeDaysModel> GetAsync(int id)
    {
        return await _api.GetAsync<FreeDaysModel>($"freedays/{id}");
    }

    public async Task<FreeDaysModel> PostAsync(FreeDaysModel body)
    {
        return await _api.PostAsync<FreeDaysModel>("freedays", body);
    }

    public async Task<FreeDaysModel> PutAsync(FreeDaysModel body)
    {
        return await _api.PutAsync<FreeDaysModel>("freedays", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"freedays/{id}");
    }

}
