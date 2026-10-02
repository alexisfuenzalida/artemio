using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using MinimalAPIArtemio.Models;
using MinimalAPIArtemio.Options;

namespace MinimalAPIArtemio.Helpers;

public class ApiHandler
{
    private readonly GraphOptions _graph;

    public ApiHandler(IOptions<GraphOptions> graph)
    {
        _graph = graph.Value;
    }

    public static void InitializeClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<AccessTokenModel?> GetAccessTokenAsync(HttpClient client)
    {
        var url = $"https://login.microsoftonline.com/{_graph.TenantId}/oauth2/v2.0/token";
        var data = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "scope", "https://outlook.office.com/SMTP.Send" },
            { "client_id", _graph.ClientId },
            { "client_secret", _graph.ClientSecret }
        };

        var response = await client.PostAsync(url, new FormUrlEncodedContent(data));
        var content = await response.Content.ReadAsStringAsync();
        return System.Text.Json.JsonSerializer.Deserialize<AccessTokenModel>(content);
    }
}
