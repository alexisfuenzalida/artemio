using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MinimalAPIArtemio.Tests;

public class AuthApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:Default"] = "Server=localhost;Port=3306;Database=artemio;Uid=root;Pwd=CHANGE_ME;",
                    ["Jwt:Key"] = "TEST-ONLY-CHANGE-ME-32CHARS-MINIMUM!!",
                    ["Jwt:Issuer"] = "MinimalAPIArtemio",
                    ["Jwt:Audience"] = "ArtemioBlazorWA",
                    ["Cors:Origins:0"] = "https://localhost:7005"
                });
            });
        }).CreateClient();
    }

    [Fact]
    public async Task Health_IsAnonymous()
    {
        var response = await _client.GetAsync("/health");
        Assert.True(response.StatusCode is HttpStatusCode.OK or HttpStatusCode.ServiceUnavailable);
    }

    [Fact]
    public async Task ProtectedEndpoint_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/api/agents");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithoutBody_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/login", new { User = "", Password = "" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
