using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ContractConditionService
{
    private readonly ApiClient _api;

    public ContractConditionService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ContractConditionModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ContractConditionModel>("contractsconditions") ?? new List<ContractConditionModel>();
    }

    public async Task<List<ContractConditionModel>> GetAsync(int id)
    {
        return await _api.GetListAsync<ContractConditionModel>($"contractsconditions/{id}") ?? new List<ContractConditionModel>();
    }

    public async Task<ContractConditionModel> PostAsync(ContractConditionModel body)
    {
        return await _api.PostAsync<ContractConditionModel>("contractsconditions", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"contractsconditions/{id}");
    }

}
