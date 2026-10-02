using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class TransportCompanyService
{
    private readonly ApiClient _api;

    public TransportCompanyService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<TransportCompanyModel>> GetAllAsync()
    {
        return await _api.GetListAsync<TransportCompanyModel>("transportcompanies") ?? new List<TransportCompanyModel>();
    }

    public async Task<TransportCompanyModel> GetAsync(int id)
    {
        return await _api.GetAsync<TransportCompanyModel>($"transportcompanies/{id}");
    }

    public async Task<TransportCompanyModel> PostAsync(TransportCompanyModel body)
    {
        return await _api.PostAsync<TransportCompanyModel>("transportcompanies", body);
    }

    public async Task<TransportCompanyModel> PutAsync(TransportCompanyModel body)
    {
        return await _api.PutAsync<TransportCompanyModel>("transportcompanies", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"transportcompanies/{id}");
    }

}
