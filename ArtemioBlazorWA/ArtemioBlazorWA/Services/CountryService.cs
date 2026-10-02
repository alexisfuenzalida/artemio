using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class CountryService
{
    private readonly ApiClient _api;

    public CountryService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<CountryModel>> GetAllAsync()
    {
        return await _api.GetListAsync<CountryModel>("countries") ?? new List<CountryModel>();
    }

    public async Task<CountryModel> GetAsync(int id)
    {
        return await _api.GetAsync<CountryModel>($"countries/{id}");
    }

    public async Task<CountryModel> PostAsync(CountryModel body)
    {
        return await _api.PostAsync<CountryModel>("countries", body);
    }

    public async Task<CountryModel> PutAsync(CountryModel body)
    {
        return await _api.PutAsync<CountryModel>("countries", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"countries/{id}");
    }

}
