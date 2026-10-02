using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class BookingDtlService
{
    private readonly ApiClient _api;

    public BookingDtlService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<BookingDetailModel>> GetAllAsync()
    {
        return await _api.GetListAsync<BookingDetailModel>("bookingdetails") ?? new List<BookingDetailModel>();
    }

    public async Task<List<BookingDetailModel>> GetAsync(int id)
    {
        return await _api.GetListAsync<BookingDetailModel>($"bookingdetails/{id}") ?? new List<BookingDetailModel>();
    }

    public async Task<BookingDetailModel> PostAsync(BookingDetailModel body)
    {
        return await _api.PostAsync<BookingDetailModel>("bookingdetails", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"bookingdetails/{id}");
    }

}
