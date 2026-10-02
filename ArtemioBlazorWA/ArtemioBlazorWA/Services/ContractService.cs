using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ContractService
{
    private readonly ApiClient _api;

    public ContractService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ContractModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ContractModel>("contracts") ?? new List<ContractModel>();
    }

    public async Task<List<ContractGrid>> GetAllGridAsync()
    {
        return await _api.GetListAsync<ContractGrid>("contractsgrid") ?? new List<ContractGrid>();
    }

    public async Task<ContractModel> GetAsync(int id)
    {
        return await _api.GetAsync<ContractModel>($"contracts/{id}");
    }

    public async Task<ContractModel> PostCopyAsync(int id)
    {
        return await _api.PostAsync<ContractModel>($"copycontracts/{id}", id);
    }

    public async Task<ContractModel> PostAsync(ContractModel body)
    {
        return await _api.PostAsync<ContractModel>("contracts", body);
    }

    public async Task<ContractModel> PutAsync(ContractModel body)
    {
        return await _api.PutAsync<ContractModel>("contracts", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"contracts/{id}");
    }

}
