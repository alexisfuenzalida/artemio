using System.Net;
using ArtemioBlazorWA.Auth;
using Xunit;

namespace ArtemioBlazorWA.Tests;

public class AuthorizationHeaderHandlerTests
{
    [Fact]
    public async Task UnauthorizedResponse_NotifiesTokenStore()
    {
        var store = new AuthTokenStore { Token = "stale-token" };
        var invoked = false;
        store.Unauthorized += () =>
        {
            invoked = true;
            return Task.CompletedTask;
        };

        var handler = new AuthorizationHeaderHandler(store, new NullLocalStorage(), new BrowserSessionStorage(new NullJSRuntime()))
        {
            InnerHandler = new StubHandler(HttpStatusCode.Unauthorized)
        };
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };

        var response = await client.GetAsync("api/booking");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.True(invoked);
        Assert.Null(store.Token);
    }

    [Fact]
    public async Task UnauthorizedLogin_DoesNotNotify()
    {
        var store = new AuthTokenStore { Token = "stale-token" };
        var invoked = false;
        store.Unauthorized += () =>
        {
            invoked = true;
            return Task.CompletedTask;
        };

        var handler = new AuthorizationHeaderHandler(store, new NullLocalStorage(), new BrowserSessionStorage(new NullJSRuntime()))
        {
            InnerHandler = new StubHandler(HttpStatusCode.Unauthorized)
        };
        using var client = new HttpClient(handler) { BaseAddress = new Uri("http://localhost/") };

        await client.PostAsync("api/login", null);

        Assert.False(invoked);
        Assert.Equal("stale-token", store.Token);
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _status;

        public StubHandler(HttpStatusCode status) => _status = status;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(new HttpResponseMessage(_status));
    }
}
