using ArtemioBlazorWA.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace ArtemioBlazorWA.Services;

public class ItineraryService
{
    private readonly ApiClient _api;

    public ItineraryService(ApiClient api)
    {
        _api = api;
    }

    public async Task<List<ItineraryGrid>> GetAllAsync()
    {
        return await _api.GetListAsync<ItineraryGrid>("itinerariesgrid") ?? new List<ItineraryGrid>();
    }

    public async Task<ItineraryModel> GetAsync(int id)
    {
        return await _api.GetAsync<ItineraryModel>($"itineraries/{id}");
    }

    public async Task<ItineraryModel> PostCopyAsync(int id, int add)
    {
        return await _api.PostAsync<ItineraryModel>($"copyitineraries/{id}/{add}", id);
    }

    public async Task<ItineraryModel> PostAsync(ItineraryModel body)
    {
        return await _api.PostAsync<ItineraryModel>("itineraries", body);
    }

    public async Task<ItineraryModel> PutAsync(ItineraryModel body)
    {
        return await _api.PutAsync<ItineraryModel>("itineraries", body);
    }

    public async Task<MessageModel> DeleteAsync(int id)
    {
        return await _api.DeleteAsync<MessageModel>($"itineraries/{id}");
    }

    public async Task<(ItineraryImportResultModel? Result, string Error)> PostImportAsync(IBrowserFile file, int seasonId)
    {
        using var content = new MultipartFormDataContent();
        using var fileStream = file.OpenReadStream(maxAllowedSize: 10_000_000);
        using var streamContent = new StreamContent(fileStream);
        content.Add(streamContent, "file", file.Name);
        content.Add(new StringContent(seasonId.ToString()), "seasonId");

        return await _api.PostFileResultAsync<ItineraryImportResultModel>("itinerariesimport", content);
    }

}
