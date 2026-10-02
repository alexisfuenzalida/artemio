using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class InvoiceDetailService
{
    private readonly ApiClient _api;

    public InvoiceDetailService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<InvoiceDetailModel>> GetAsync(int invoiceId)
    {
        return await _api.GetListAsync<InvoiceDetailModel>($"invoicesdetails/{invoiceId}") ?? new List<InvoiceDetailModel>();
    }

    public async Task<InvoiceDetailModel> PostAsync(InvoiceDetailModel body)
    {
        return await _api.PostAsync<InvoiceDetailModel>("invoicesdetails", body);
    }

    public async Task<MessageModel> DeleteAsync(int invoiceId)
    {
        return await _api.DeleteAsync<MessageModel>($"invoicesdetails/{invoiceId}");
    }
}
