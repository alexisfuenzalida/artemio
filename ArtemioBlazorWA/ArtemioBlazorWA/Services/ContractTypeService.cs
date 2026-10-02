using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ContractTypeService
{
    private readonly ApiClient _api;

    public ContractTypeService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ContractTypeModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ContractTypeModel>("contracttypes") ?? new List<ContractTypeModel>();
    }

    public async Task<ContractTypeModel> GetAsync(int id)
    {
        return await _api.GetAsync<ContractTypeModel>($"contracttypes/{id}");
    }

    public async Task<ContractTypeModel> PostAsync(ContractTypeModel body)
    {
        return await _api.PostAsync<ContractTypeModel>("contracttypes", body);
    }

    public async Task<ContractTypeModel> PutAsync(ContractTypeModel body)
    {
        return await _api.PutAsync<ContractTypeModel>("contracttypes", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"contracttypes/{id}");
    }

}
