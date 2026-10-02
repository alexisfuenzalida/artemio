using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class NotifyService
{
    private readonly ApiClient _api;

    public NotifyService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<NotifyModel>> GetAllAsync()
    {
        return await _api.GetListAsync<NotifyModel>("notifies") ?? new List<NotifyModel>();
    }

    public async Task<NotifyModel> GetAsync(int id)
    {
        return await _api.GetAsync<NotifyModel>($"notifies/{id}");
    }

    public async Task<NotifyModel> PostAsync(NotifyModel body)
    {
        return await _api.PostAsync<NotifyModel>("notifies", body);
    }

    public async Task<NotifyModel> PutAsync(NotifyModel body)
    {
        return await _api.PutAsync<NotifyModel>("notifies", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"notifies/{id}");
    }

}
