using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ArtemioBlazorWA.Auth;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private const string TokenKey = "authToken";

    private readonly ILocalStorageService _localStorage;
    private readonly BrowserSessionStorage _sessionStorage;
    private readonly AuthTokenStore _tokenStore;

    public JwtAuthStateProvider(ILocalStorageService localStorage, BrowserSessionStorage sessionStorage, AuthTokenStore tokenStore)
    {
        _localStorage = localStorage;
        _sessionStorage = sessionStorage;
        _tokenStore = tokenStore;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = _tokenStore.Token;
        if (string.IsNullOrWhiteSpace(token))
        {
            try
            {
                // "Remember me" tokens live in localStorage (survive a browser restart); tokens
                // for a session that should be asked again later live only in sessionStorage
                // (cleared when the tab/browser closes).
                token = await _localStorage.GetItemAsync<string>(TokenKey);
                if (string.IsNullOrWhiteSpace(token))
                {
                    token = await _sessionStorage.GetItemAsync(TokenKey);
                }
                _tokenStore.Token = token;
            }
            catch (InvalidOperationException)
            {
                return Anonymous();
            }
            catch (JSException)
            {
                return Anonymous();
            }
        }

        if (!JwtTokenValidator.TryGetClaims(token, out var claims))
        {
            await ClearStoredTokenAsync();
            return Anonymous();
        }

        var identity = new ClaimsIdentity(claims, "jwt");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public async Task MarkUserAsAuthenticated(string token, bool remember)
    {
        _tokenStore.Token = token;
        _tokenStore.ResetUnauthorizedHandling();

        if (remember)
        {
            await _localStorage.SetItemAsync(TokenKey, token);
            await _sessionStorage.RemoveItemAsync(TokenKey);
        }
        else
        {
            await _sessionStorage.SetItemAsync(TokenKey, token);
            await _localStorage.RemoveItemAsync(TokenKey);
        }

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task MarkUserAsLoggedOut()
    {
        await ClearStoredTokenAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous()));
    }

    public void NotifyUserChanged() => NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

    private async Task ClearStoredTokenAsync()
    {
        _tokenStore.Token = null;
        try
        {
            await _localStorage.RemoveItemAsync(TokenKey);
            await _sessionStorage.RemoveItemAsync(TokenKey);
        }
        catch (InvalidOperationException)
        {
        }
        catch (JSException)
        {
        }
    }

    private static AuthenticationState Anonymous() =>
        new(new ClaimsPrincipal(new ClaimsIdentity()));
}
