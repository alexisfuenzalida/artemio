using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class PaymentService
{
    private readonly ApiClient _api;

    public PaymentService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<PaymentGrid>> GetAllAsync(int season)
    {
        return await _api.GetListAsync<PaymentGrid>($"paymentsgrid/{season}") ?? new List<PaymentGrid>();
    }

    public async Task<PaymentModel> GetAsync(int id)
    {
        return await _api.GetAsync<PaymentModel>($"payments/{id}");
    }

    public async Task<PaymentModel> PostAsync(PaymentModel body)
    {
        return await _api.PostAsync<PaymentModel>("payments", body);
    }

    public async Task<PaymentModel> PutAsync(PaymentModel body)
    {
        return await _api.PutAsync<PaymentModel>("payments", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"payments/{id}");
    }
}
