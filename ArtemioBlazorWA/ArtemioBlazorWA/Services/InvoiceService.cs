using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class InvoiceService
{
    private readonly ApiClient _api;

    public InvoiceService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<InvoiceGrid>> GetAllAsync(int season)
    {
        return await _api.GetListAsync<InvoiceGrid>($"invoicesgrid/{season}") ?? new List<InvoiceGrid>();
    }

    public async Task<InvoiceModel> GetAsync(int id)
    {
        return await _api.GetAsync<InvoiceModel>($"invoices/{id}");
    }

    public async Task<List<InvoiceBalanceModel>> GetBySeasonAsync(int seasonId, int paymentId = 0)
    {
        return await _api.GetListAsync<InvoiceBalanceModel>($"invoices/season/{seasonId}/{paymentId}") ?? new List<InvoiceBalanceModel>();
    }

    public async Task<InvoiceModel> PostAsync(InvoiceModel body)
    {
        return await _api.PostAsync<InvoiceModel>("invoices", body);
    }

    public async Task<InvoiceModel> PutAsync(InvoiceModel body)
    {
        return await _api.PutAsync<InvoiceModel>("invoices", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"invoices/{id}");
    }

}
