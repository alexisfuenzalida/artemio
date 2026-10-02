using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class AgentAduanaService
{
    private readonly ApiClient _api;

    public AgentAduanaService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<AgentModel>> GetAllAsync()
    {
        return await _api.GetListAsync<AgentModel>("agentsaduanas") ?? new List<AgentModel>();
    }

    public async Task<AgentModel> GetAsync(int id)
    {
        return await _api.GetAsync<AgentModel>($"agentsaduanas/{id}");
    }

    public async Task<AgentModel> PostAsync(AgentModel body)
    {
        return await _api.PostAsync<AgentModel>("agentsaduanas", body);
    }

    public async Task<AgentModel> PutAsync(AgentModel body)
    {
        return await _api.PutAsync<AgentModel>("agentsaduanas", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"agentsaduanas/{id}");
    }

}
