using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class TransportCompanyItemService
{
    private readonly ApiClient _api;

    public TransportCompanyItemService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<TransportCompanyItemModel>> GetAsync(int id)
    {
        return await _api.GetListAsync<TransportCompanyItemModel>($"tci/{id}") ?? new List<TransportCompanyItemModel>();
    }

    public async Task<TransportCompanyItemModel> PostAsync(TransportCompanyItemModel body)
    {
        return await _api.PostAsync<TransportCompanyItemModel>("tci", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"tci/{id}");
    }

}
