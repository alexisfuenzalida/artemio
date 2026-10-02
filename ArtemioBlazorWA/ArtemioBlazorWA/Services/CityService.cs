using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class CityService
{
    private readonly ApiClient _api;

    public CityService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<CityModel>> GetAllAsync()
    {
        return await _api.GetListAsync<CityModel>("cities") ?? new List<CityModel>();
    }

    public async Task<CityModel> GetAsync(int id)
    {
        return await _api.GetAsync<CityModel>($"cities/{id}");
    }

    public async Task<CityModel> PostAsync(CityModel body)
    {
        return await _api.PostAsync<CityModel>("cities", body);
    }

    public async Task<CityModel> PutAsync(CityModel body)
    {
        return await _api.PutAsync<CityModel>("cities", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"cities/{id}");
    }

}
