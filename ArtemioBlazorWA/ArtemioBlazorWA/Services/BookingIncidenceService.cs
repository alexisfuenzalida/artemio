using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class BookingIncidenceService
{
    private readonly ApiClient _api;

    public BookingIncidenceService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<BookingIncidenceGrid>> GetAllAsync(int id)
    {
        return await _api.GetListAsync<BookingIncidenceGrid>($"bookingincidencesgrid/{id}") ?? new List<BookingIncidenceGrid>();
    }

    public async Task<List<BookingIncidenceModel>> GetAsync(int id,string container)
    {
        return await _api.GetListAsync<BookingIncidenceModel>($"bookingincidences/{id}/{container}") ?? new List<BookingIncidenceModel>();
    }

    public async Task<BookingIncidenceModel> PostAsync(BookingIncidenceModel body)
    {
        return await _api.PostAsync<BookingIncidenceModel>("bookingincidences", body);
    }

    public async Task<MessageModel> DeleteAsync(int id, string container)
    {
        return await _api.DeleteAsync<MessageModel>($"bookingincidences/{id}/{container}");
    }

}
