using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ShipperContactService
{
    private readonly ApiClient _api;

    public ShipperContactService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ShipperContactModel>> GetAsync(int id)
    {
        return await _api.GetListAsync<ShipperContactModel>($"shipperscontacts/{id}") ?? new List<ShipperContactModel>();
    }

    public async Task<ShipperContactModel> PostAsync(ShipperContactModel body)
    {
        return await _api.PostAsync<ShipperContactModel>("shipperscontacts", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"shipperscontacts/{id}");
    }

}
