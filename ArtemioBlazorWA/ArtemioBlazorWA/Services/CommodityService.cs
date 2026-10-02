using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class CommodityService
{
    private readonly ApiClient _api;

    public CommodityService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<CommodityModel>> GetAllAsync()
    {
        return await _api.GetListAsync<CommodityModel>("commodities") ?? new List<CommodityModel>();
    }

    public async Task<CommodityModel> GetAsync(int id)
    {
        return await _api.GetAsync<CommodityModel>($"commodities/{id}");
    }

    public async Task<CommodityModel> PostAsync(CommodityModel body)
    {
        return await _api.PostAsync<CommodityModel>("commodities", body);
    }

    public async Task<CommodityModel> PutAsync(CommodityModel body)
    {
        return await _api.PutAsync<CommodityModel>("commodities", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"commodities/{id}");
    }

}
