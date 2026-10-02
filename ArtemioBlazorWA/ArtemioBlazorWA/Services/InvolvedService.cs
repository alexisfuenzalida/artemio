using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class InvolvedService
{
    private readonly ApiClient _api;

    public InvolvedService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<InvolvedModel>> GetAllAsync()
    {
        return await _api.GetListAsync<InvolvedModel>("involved") ?? new List<InvolvedModel>();
    }

    public async Task<InvolvedModel> GetAsync(int id)
    {
        return await _api.GetAsync<InvolvedModel>($"involved/{id}");
    }

    public async Task<InvolvedModel> PostAsync(InvolvedModel body)
    {
        return await _api.PostAsync<InvolvedModel>("involved", body);
    }

    public async Task<InvolvedModel> PutAsync(InvolvedModel body)
    {
        return await _api.PutAsync<InvolvedModel>("involved", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"involved/{id}");
    }

}
