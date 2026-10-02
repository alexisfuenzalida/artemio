using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class WarehouseService
{
    private readonly ApiClient _api;

    public WarehouseService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<WarehouseModel>> GetAllAsync()
    {
        return await _api.GetListAsync<WarehouseModel>("warehouses") ?? new List<WarehouseModel>();
    }

    public async Task<WarehouseModel> GetAsync(int id)
    {
        return await _api.GetAsync<WarehouseModel>($"warehouses/{id}");
    }

    public async Task<WarehouseModel> PostAsync(WarehouseModel body)
    {
        return await _api.PostAsync<WarehouseModel>("warehouses", body);
    }

    public async Task<WarehouseModel> PutAsync(WarehouseModel body)
    {
        return await _api.PutAsync<WarehouseModel>("warehouses", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"warehouses/{id}");
    }

}
