using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class IncidenceTypeService
{
    private readonly ApiClient _api;

    public IncidenceTypeService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<IncidenceTypeModel>> GetAllAsync()
    {
        return await _api.GetListAsync<IncidenceTypeModel>("incidencestype") ?? new List<IncidenceTypeModel>();
    }

    public async Task<IncidenceTypeModel> GetAsync(int id)
    {
        return await _api.GetAsync<IncidenceTypeModel>($"incidencestype/{id}");
    }

    public async Task<IncidenceTypeModel> PostAsync(IncidenceTypeModel body)
    {
        return await _api.PostAsync<IncidenceTypeModel>("incidencestype", body);
    }

    public async Task<IncidenceTypeModel> PutAsync(IncidenceTypeModel body)
    {
        return await _api.PutAsync<IncidenceTypeModel>("incidencestype", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"incidencestype/{id}");
    }

}
