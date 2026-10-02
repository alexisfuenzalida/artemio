using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class BookingService
{
    private readonly ApiClient _api;

    public BookingService(ApiClient api)
    {
        _api = api;
    }

    public async Task<int> GetBookingTTAsync(int ServiceId, int POLId, DateTime? ETD, int PODId)
    {
        return await _api.GetAsync<int?>($"bookingtt/{ServiceId}/{POLId}/{ETD}/{PODId}") ?? 0;
    }

    public async Task<List<BookingContractGridModel>> GetAllBookingContractAsync(int POLId,int PODId, int TransportCompanyId)
    {
        return await _api.GetListAsync<BookingContractGridModel>($"bookingcontractgrid/{POLId}/{PODId}/{TransportCompanyId}") ?? new List<BookingContractGridModel>();
    }

    public async Task<List<BookingModel>> GetAllAsync()
    {
        return await _api.GetListAsync<BookingModel>("booking") ?? new List<BookingModel>();
    }

    public async Task<List<BookingGrid>> GetAllGridAsync(int id)
    {
        return await _api.GetListAsync<BookingGrid>($"bookinggrid/{id}") ?? new List<BookingGrid>();
    }

    public async Task<(List<BookingGrid> Data, string? Error)> GetBookingSendDaysGridAsync()
    {
        var (data, error) = await _api.GetListResultAsync<BookingGrid>("bookingsenddaysgrid");
        return (data ?? new List<BookingGrid>(), error);
    }

    public async Task<BookingModel> PostCopyAsync(int id)
    {
        return await _api.PostAsync<BookingModel>($"copybooking/{id}", id);
    }

    public async Task<BookingModel> GetAsync(int id)
    {
        return await _api.GetAsync<BookingModel>($"booking/{id}");
    }

    public async Task<(BookingModel? Data, string Error)> PostResultAsync(BookingModel body)
    {
        return await _api.PostResultAsync<BookingModel>("booking", body);
    }

    public async Task<(BookingModel? Data, string Error)> PutResultAsync(BookingModel body)
    {
        return await _api.PutResultAsync<BookingModel>("booking", body);
    }

    public async Task<MessageModel> PostSendBookingAsync(BookingIdModel body)
    {
        return await _api.PostAsync<MessageModel>("sendbooking", body);
    }

    public async Task<(bool Ok, string Message)> PostSendBookingFreeDaysAsync(BookingSendDaysRequestModel body)
    {
        return await _api.PostMessageAsync("bookingsenddays", body);
    }

    public async Task<BookingModel> PutAsync(BookingModel body)
    {
        return await _api.PutAsync<BookingModel>("booking", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"booking/{id}");
    }

    public async Task<List<BookingStackingGrid>> GetBookingStackingAsync()
    {
        return await _api.GetListAsync<BookingStackingGrid>("bookingstacking") ?? new List<BookingStackingGrid>();
    }

    public async Task<bool> PutBookingStackingAsync(List<BookingStackingGrid> body)
    {
        return await _api.PutAsync<bool>("bookingstacking", body);
    }

    public async Task<List<BookingHsCodeGrid>> GetBookingHsCodeAsync()
    {
        return await _api.GetListAsync<BookingHsCodeGrid>("bookinghscode") ?? new List<BookingHsCodeGrid>();
    }

    public async Task<bool> PutBookingHsCodeAsync(List<BookingHsCodeGrid> body)
    {
        return await _api.PutAsync<bool>("bookinghscode", body);
    }

}
