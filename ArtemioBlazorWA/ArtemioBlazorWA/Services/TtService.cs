using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class TtService
{
    private readonly ApiClient _api;

    public TtService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<TtModel>> GetAllAsync()
    {
        return await _api.GetListAsync<TtModel>("tts") ?? new List<TtModel>();
    }

    public async Task<List<TtDetailModel>> GetAllDetailAsync(int id)
    {
        return await _api.GetListAsync<TtDetailModel>($"ttsdetails/{id}") ?? new List<TtDetailModel>();
    }

    public async Task<List<TtDetailModel>> GetSearchDetailAsync(int serviceId,int polId)
    {
        return await _api.GetListAsync<TtDetailModel>($"ttssearch/{serviceId}/{polId}") ?? new List<TtDetailModel>();
    }

    public async Task<TtModel> GetAsync(int id)
    {
        return await _api.GetAsync<TtModel>($"tts/{id}");
    }

    public async Task<TtModel> PostAsync(TtModel body)
    {
        return await _api.PostAsync<TtModel>("tts", body);
    }

    public async Task<TtDetailModel> PostDetailAsync(TtDetailModel body)
    {
        return await _api.PostAsync<TtDetailModel>("ttsdetails", body);
    }

    public async Task<TtModel> PutAsync(TtModel body)
    {
        return await _api.PutAsync<TtModel>("tts", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"tts/{id}");
    }

    public async Task<MessageModel> DeleteDetailAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"ttsdetails/{id}");
    }

}
