using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class CostItemService
{
    private readonly ApiClient _api;

    public CostItemService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<CostItemModel>> GetAllAsync()
    {
        return await _api.GetListAsync<CostItemModel>("costitems") ?? new List<CostItemModel>();
    }

    public async Task<CostItemModel> GetAsync(int id)
    {
        return await _api.GetAsync<CostItemModel>($"costitems/{id}");
    }

    public async Task<CostItemModel> PostAsync(CostItemModel body)
    {
        return await _api.PostAsync<CostItemModel>("costitems", body);
    }

    public async Task<CostItemModel> PutAsync(CostItemModel body)
    {
        return await _api.PutAsync<CostItemModel>("costitems", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"costitems/{id}");
    }

}
