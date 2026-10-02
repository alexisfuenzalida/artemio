using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ContainerTypeService
{
    private readonly ApiClient _api;

    public ContainerTypeService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ContainerTypeModel>> GetAllAsync()
    {
        return await _api.GetListAsync<ContainerTypeModel>("containerstype") ?? new List<ContainerTypeModel>();
    }

    public async Task<ContainerTypeModel> GetAsync(int id)
    {
        return await _api.GetAsync<ContainerTypeModel>($"containerstype/{id}");
    }

    public async Task<ContainerTypeModel> PostAsync(ContainerTypeModel body)
    {
        return await _api.PostAsync<ContainerTypeModel>("containerstype", body);
    }

    public async Task<ContainerTypeModel> PutAsync(ContainerTypeModel body)
    {
        return await _api.PutAsync<ContainerTypeModel>("containerstype", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"containerstype/{id}");
    }

}
