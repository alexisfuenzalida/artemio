using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class PaymentConditionService
{
    private readonly ApiClient _api;

    public PaymentConditionService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<PaymentConditionModel>> GetAllAsync()
    {
        return await _api.GetListAsync<PaymentConditionModel>("paymentconditions") ?? new List<PaymentConditionModel>();
    }

    public async Task<PaymentConditionModel> GetAsync(int id)
    {
        return await _api.GetAsync<PaymentConditionModel>($"paymentconditions/{id}");
    }

    public async Task<PaymentConditionModel> PostAsync(PaymentConditionModel body)
    {
        return await _api.PostAsync<PaymentConditionModel>("paymentconditions", body);
    }

    public async Task<PaymentConditionModel> PutAsync(PaymentConditionModel body)
    {
        return await _api.PutAsync<PaymentConditionModel>("paymentconditions", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"paymentconditions/{id}");
    }

}
