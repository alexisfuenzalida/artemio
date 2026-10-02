using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ConsigneeService
{
    private readonly ApiClient _api;

    public ConsigneeService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ConsigneeModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ConsigneeModel>("consignees") ?? new List<ConsigneeModel>();
    }

    public async Task<ConsigneeModel> GetAsync(int id)
    {
        return await _api.GetAsync<ConsigneeModel>($"consignees/{id}");
    }

    public async Task<ConsigneeModel> PostAsync(ConsigneeModel body)
    {
        return await _api.PostAsync<ConsigneeModel>("consignees", body);
    }

    public async Task<ConsigneeModel> PutAsync(ConsigneeModel body)
    {
        return await _api.PutAsync<ConsigneeModel>("consignees", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"consignees/{id}");
    }

}
