using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class PreInvoiceService
{
    private readonly ApiClient _api;

    public PreInvoiceService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<PreInvoiceModel>> GetByBookingAsync(int bookingId)
    {
        return await _api.GetListAsync<PreInvoiceModel>($"preinvoices/booking/{bookingId}") ?? new List<PreInvoiceModel>();
    }

    public async Task<List<PreInvoiceModel>> GetBySeasonAndTypeAsync(int seasonId, string type, int invoiceId = 0)
    {
        return await _api.GetListAsync<PreInvoiceModel>($"preinvoices/season/{seasonId}/type/{Uri.EscapeDataString(type)}?invoiceId={invoiceId}")
            ?? new List<PreInvoiceModel>();
    }

    public async Task<PreInvoiceModel> PostAsync(PreInvoiceModel body)
    {
        return await _api.PostAsync<PreInvoiceModel>("preinvoices", body);
    }

    public async Task<PreInvoiceModel> PutAsync(PreInvoiceModel body)
    {
        return await _api.PutAsync<PreInvoiceModel>("preinvoices", body);
    }

    public async Task<MessageModel> DeleteAsync(int bookingId, string type)
    {
        return await _api.DeleteAsync<MessageModel>($"preinvoices/{bookingId}/{type}");
    }

    public async Task<MessageModel> DeleteByBookingAsync(int bookingId)
    {
        return await _api.DeleteAsync<MessageModel>($"preinvoices/booking/{bookingId}");
    }

}
