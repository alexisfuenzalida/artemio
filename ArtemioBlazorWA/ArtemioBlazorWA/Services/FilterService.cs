using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class FilterService
{
    private readonly ApiClient _api;

    public FilterService(ApiClient api)
    {
        _api = api;
    }

    public async Task<Stream> PostContractAsync(ContractFilterModel body)
    {
        return await _api.PostStreamAsync("contractsfilters", body);
    }

    public async Task<(Stream StreamResult, MessageModel StringResult)> PostItineraryAsync(ItineraryFilterModel body)
    {
        var stream = await _api.PostStreamAsync("itinerariesfilters", body);
        return stream is null ? default : (stream, null);
    }

}
