using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class IntermediaryService
{
    private readonly ApiClient _api;

    public IntermediaryService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<IntermediaryModel>> GetAllAsync()
    {
        return await _api.GetListAsync<IntermediaryModel>("intermediaries") ?? new List<IntermediaryModel>();
    }

    public async Task<IntermediaryModel> GetAsync(int id)
    {
        return await _api.GetAsync<IntermediaryModel>($"intermediaries/{id}");
    }

    public async Task<IntermediaryModel> PostAsync(IntermediaryModel body)
    {
        return await _api.PostAsync<IntermediaryModel>("intermediaries", body);
    }

    public async Task<IntermediaryModel> PutAsync(IntermediaryModel body)
    {
        return await _api.PutAsync<IntermediaryModel>("intermediaries", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"intermediaries/{id}");
    }

}
