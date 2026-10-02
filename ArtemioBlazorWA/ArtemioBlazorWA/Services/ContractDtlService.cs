using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ContractDtlService
{
    private readonly ApiClient _api;

    public ContractDtlService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ContractDetailModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ContractDetailModel>("contractsdetails") ?? new List<ContractDetailModel>();
    }

    public async Task<List<ContractDetailModel>> GetAsync(int id)
    {
        return await _api.GetListAsync<ContractDetailModel>($"contractsdetails/{id}") ?? new List<ContractDetailModel>();
    }

    public async Task<ContractDetailModel> PostAsync(ContractDetailModel body)
    {
        return await _api.PostAsync<ContractDetailModel>("contractsdetails", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"contractsdetails/{id}");
    }

}
