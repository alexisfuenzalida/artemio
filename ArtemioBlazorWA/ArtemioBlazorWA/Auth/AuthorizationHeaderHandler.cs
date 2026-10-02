using System.Net;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.JSInterop;

namespace ArtemioBlazorWA.Auth;

public class AuthorizationHeaderHandler : DelegatingHandler
{
    private const string TokenKey = "authToken";

    private readonly AuthTokenStore _tokenStore;
    private readonly ILocalStorageService _localStorage;
    private readonly BrowserSessionStorage _sessionStorage;

    public AuthorizationHeaderHandler(AuthTokenStore tokenStore, ILocalStorageService localStorage, BrowserSessionStorage sessionStorage)
    {
        _tokenStore = tokenStore;
        _localStorage = localStorage;
        _sessionStorage = sessionStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var skipAuth = IsAnonymousEndpoint(request);

        if (!skipAuth)
        {
            var token = await GetTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        var response = await base.SendAsync(request, cancellationToken);

        if (!skipAuth && response.StatusCode == HttpStatusCode.Unauthorized)
        {
            _tokenStore.NotifyUnauthorized();
        }

        return response;
    }

    private async Task<string?> GetTokenAsync()
    {
        var token = _tokenStore.Token;
        if (!string.IsNullOrWhiteSpace(token))
        {
            return token;
        }

        try
        {
            token = await _localStorage.GetItemAsync<string>(TokenKey);
            if (string.IsNullOrWhiteSpace(token))
            {
                token = await _sessionStorage.GetItemAsync(TokenKey);
            }
            _tokenStore.Token = token;
            return token;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
        catch (JSException)
        {
            return null;
        }
    }

    private static bool IsAnonymousEndpoint(HttpRequestMessage request)
    {
        var path = request.RequestUri?.AbsolutePath ?? string.Empty;
        return path.EndsWith("/login", StringComparison.OrdinalIgnoreCase)
            || path.EndsWith("/seasons", StringComparison.OrdinalIgnoreCase)
            || path.EndsWith("/health", StringComparison.OrdinalIgnoreCase);
    }
}
