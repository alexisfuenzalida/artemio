using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class TransportCompanyServiceService
{
    private readonly ApiClient _api;

    public TransportCompanyServiceService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ServiceModel>> GetAllAssignedAsync(int id)
    {
        return await _api.GetListAsync<ServiceModel>($"tcsassigned/{id}") ?? new List<ServiceModel>();
    }

    public async Task<List<ServiceModel>> GetAllAvailableAsync(int id)
    {
        return await _api.GetListAsync<ServiceModel>($"tcsavailable/{id}") ?? new List<ServiceModel>();
    }

    public async Task<TransportCompanyServiceModel> PostAsync(TransportCompanyServiceModel body)
    {
        return await _api.PostAsync<TransportCompanyServiceModel>("tcs", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"tcs/{id}");
    }

}
