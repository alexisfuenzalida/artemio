using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class BookingPODService
{
    private readonly ApiClient _api;

    public BookingPODService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<BookingPODModel>> GetAllAsync(int id)
    {
        return await _api.GetListAsync<BookingPODModel>($"bookingpod/{id}") ?? new List<BookingPODModel>();
    }

    public async Task<BookingPODModel> PostAsync(BookingPODModel body)
    {
        return await _api.PostAsync<BookingPODModel>("bookingpod", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"bookingpod/{id}");
    }

}
