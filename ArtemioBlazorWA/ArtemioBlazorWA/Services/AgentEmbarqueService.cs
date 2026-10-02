using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class AgentEmbarqueService
{
    private readonly ApiClient _api;

    public AgentEmbarqueService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<AgentModel>> GetAllAsync()
    {
        return await _api.GetListAsync<AgentModel>("agentsembarques") ?? new List<AgentModel>();
    }

    public async Task<AgentModel> GetAsync(int id)
    {
        return await _api.GetAsync<AgentModel>($"agentsembarques/{id}");
    }

    public async Task<AgentModel> PostAsync(AgentModel body)
    {
        return await _api.PostAsync<AgentModel>("agentsembarques", body);
    }

    public async Task<AgentModel> PutAsync(AgentModel body)
    {
        return await _api.PutAsync<AgentModel>("agentsembarques", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"agentsembarques/{id}");
    }

}
