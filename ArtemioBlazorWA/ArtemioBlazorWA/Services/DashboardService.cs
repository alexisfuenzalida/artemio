using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class DashboardService
{
    private readonly ApiClient _api;

    public DashboardService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<BookingDashboard>> GetAllAsync(int id)
    {
        return await _api.GetListAsync<BookingDashboard>($"bookingdashboard/{id}") ?? new List<BookingDashboard>();
    }

}
