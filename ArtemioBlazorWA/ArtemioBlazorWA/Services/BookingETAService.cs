using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class BookingETAService
{
    private readonly ApiClient _api;

    public BookingETAService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<BookingETAModel>> GetAllAsync(int id)
    {
        return await _api.GetListAsync<BookingETAModel>($"bookingeta/{id}") ?? new List<BookingETAModel>();
    }

    public async Task<BookingETAModel> PostAsync(BookingETAModel body)
    {
        return await _api.PostAsync<BookingETAModel>("bookingeta", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"bookingeta/{id}");
    }

}
