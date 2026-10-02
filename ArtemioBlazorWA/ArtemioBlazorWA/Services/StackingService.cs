using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class StackingService
{
    private readonly ApiClient _api;

    public StackingService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<StackingModel>> GetAllAsync()
    {
        return await _api.GetListAsync<StackingModel>("stacking") ?? new List<StackingModel>();
    }

    public async Task<StackingModel> GetAsync(int id)
    {
        return await _api.GetAsync<StackingModel>($"stacking/{id}");
    }

    public async Task<StackingModel> GetStSeAsync(int id)
    {
        return await _api.GetAsync<StackingModel>($"stackingbyservice/{id}");
    }

    public async Task<StackingModel> GetAsync(int tc, int se)
    {
        return await _api.GetAsync<StackingModel>($"stacking/{tc}/{se}");
    }

    public async Task<StackingModel> PostAsync(StackingModel body)
    {
        return await _api.PostAsync<StackingModel>("stacking", body);
    }

    public async Task<StackingModel> PutAsync(StackingModel body)
    {
        return await _api.PutAsync<StackingModel>("stacking", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"stacking/{id}");
    }

}
