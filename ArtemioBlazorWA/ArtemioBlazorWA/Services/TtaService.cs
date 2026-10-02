using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class TtaService
{
    private readonly ApiClient _api;

    public TtaService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<TtaModel>> GetAllAsync()
    {
        return await _api.GetListAsync<TtaModel>("ttas") ?? new List<TtaModel>();
    }

    public async Task<TtaModel> GetAsync(int id)
    {
        return await _api.GetAsync<TtaModel>($"ttas/{id}");
    }

    public async Task<TtaModel> PostAsync(TtaModel body)
    {
        return await _api.PostAsync<TtaModel>("ttas", body);
    }

    public async Task<TtaModel> PutAsync(TtaModel body)
    {
        return await _api.PutAsync<TtaModel>("ttas", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"ttas/{id}");
    }

}
