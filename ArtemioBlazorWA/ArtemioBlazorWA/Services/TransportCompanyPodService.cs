using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class TransportCompanyPodService
{
    private readonly ApiClient _api;

    public TransportCompanyPodService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<PortModel>> GetAllAssignedAsync(int id)
    {
        return await _api.GetListAsync<PortModel>($"tcpodassigned/{id}") ?? new List<PortModel>();
    }

    public async Task<List<PortModel>> GetAllAvailableAsync(int id)
    {
        return await _api.GetListAsync<PortModel>($"tcpodavailable/{id}") ?? new List<PortModel>();
    }

    public async Task<TransportCompanyPodModel> PostAsync(TransportCompanyPodModel body)
    {
        return await _api.PostAsync<TransportCompanyPodModel>("tcpod", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"tcpod/{id}");
    }

}
