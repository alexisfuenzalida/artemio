using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class AgentService
{
    private readonly ApiClient _api;

    public AgentService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<AgentModel>> GetAllAsync()
    {
        return await _api.GetListAsync<AgentModel>("agents") ?? new List<AgentModel>();
    }

    public async Task<AgentModel> GetAsync(int id)
    {
        return await _api.GetAsync<AgentModel>($"agents/{id}");
    }

    public async Task<AgentModel> PostAsync(AgentModel body)
    {
        return await _api.PostAsync<AgentModel>("agents", body);
    }

    public async Task<AgentModel> PutAsync(AgentModel body)
    {
        return await _api.PutAsync<AgentModel>("agents", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"agents/{id}");
    }

}
