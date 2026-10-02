using Microsoft.JSInterop;

namespace ArtemioBlazorWA.Auth;

/// <summary>
/// Thin wrapper around the browser's sessionStorage (cleared when the tab/browser closes),
/// used as the "don't remember me" counterpart to Blazored.LocalStorage.
/// </summary>
public class BrowserSessionStorage
{
    private readonly IJSRuntime _js;

    public BrowserSessionStorage(IJSRuntime js)
    {
        _js = js;
    }

    public async Task SetItemAsync(string key, string value)
    {
        try
        {
            await _js.InvokeVoidAsync("sessionStorage.setItem", key, value);
        }
        catch (InvalidOperationException) { }
        catch (JSException) { }
    }

    public async Task<string?> GetItemAsync(string key)
    {
        try
        {
            return await _js.InvokeAsync<string?>("sessionStorage.getItem", key);
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

    public async Task RemoveItemAsync(string key)
    {
        try
        {
            await _js.InvokeVoidAsync("sessionStorage.removeItem", key);
        }
        catch (InvalidOperationException) { }
        catch (JSException) { }
    }
}
