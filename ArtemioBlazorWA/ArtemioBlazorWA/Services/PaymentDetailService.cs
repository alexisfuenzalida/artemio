using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class PaymentDetailService
{
    private readonly ApiClient _api;

    public PaymentDetailService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<PaymentDetailModel>> GetAsync(int paymentId)
    {
        return await _api.GetListAsync<PaymentDetailModel>($"paymentsdetails/{paymentId}") ?? new List<PaymentDetailModel>();
    }

    public async Task<PaymentDetailModel> PostAsync(PaymentDetailModel body)
    {
        return await _api.PostAsync<PaymentDetailModel>("paymentsdetails", body);
    }

    public async Task<MessageModel> DeleteAsync(int paymentId)
    {
        return await _api.DeleteAsync<MessageModel>($"paymentsdetails/{paymentId}");
    }
}
