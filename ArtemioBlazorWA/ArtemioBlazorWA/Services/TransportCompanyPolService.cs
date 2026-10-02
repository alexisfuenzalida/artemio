using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class TransportCompanyPolService
{
    private readonly ApiClient _api;

    public TransportCompanyPolService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<PortModel>> GetAllAssignedAsync(int id)
    {
        return await _api.GetListAsync<PortModel>($"tcpolassigned/{id}") ?? new List<PortModel>();
    }

    public async Task<List<PortModel>> GetAllAvailableAsync(int id)
    {
        return await _api.GetListAsync<PortModel>($"tcpolavailable/{id}") ?? new List<PortModel>();
    }

    public async Task<TransportCompanyPolModel> PostAsync(TransportCompanyPolModel body)
    {
        return await _api.PostAsync<TransportCompanyPolModel>("tcpol", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"tcpol/{id}");
    }

}
