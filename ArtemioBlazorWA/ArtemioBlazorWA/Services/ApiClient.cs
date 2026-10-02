using System.Net.Http.Json;
using System.Text.Json;
using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class ApiClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient _http;

    public ApiClient(HttpClient http)
    {
        _http = http;
    }

    public HttpClient Http => _http;

    public async Task<T?> GetAsync<T>(string url)
    {
        using var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    public Task<List<T>?> GetListAsync<T>(string url) => GetAsync<List<T>>(url);

    public async Task<(List<T>? Data, string? Error)> GetListResultAsync<T>(string url)
    {
        using var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            var payload = default(MessageModel);
            try
            {
                payload = await response.Content.ReadFromJsonAsync<MessageModel>(JsonOptions);
            }
            catch
            {
            }

            return (null, string.IsNullOrWhiteSpace(payload?.Message)
                ? $"Error {(int)response.StatusCode}"
                : payload!.Message);
        }

        var data = await response.Content.ReadFromJsonAsync<List<T>>(JsonOptions);
        return (data, null);
    }

    public async Task<T?> PostAsync<T>(string url, object? body)
    {
        using var response = await _http.PostAsJsonAsync(url, body, JsonOptions);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        if (response.Content.Headers.ContentLength == 0)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    public async Task<(T? Data, string Error)> PostResultAsync<T>(string url, object? body)
    {
        using var response = await _http.PostAsJsonAsync(url, body, JsonOptions);
        if (!response.IsSuccessStatusCode)
        {
            return (default, await ReadErrorAsync(response, "Could not save"));
        }

        if (response.Content.Headers.ContentLength == 0)
        {
            return (default, "Empty response");
        }

        var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        return (data, "");
    }

    public async Task<(T? Data, string Error)> PutResultAsync<T>(string url, object? body)
    {
        using var response = await _http.PutAsJsonAsync(url, body, JsonOptions);
        if (!response.IsSuccessStatusCode)
        {
            return (default, await ReadErrorAsync(response, "Could not save"));
        }

        var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        return (data, "");
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response, string fallback)
    {
        try
        {
            var payload = await response.Content.ReadFromJsonAsync<MessageModel>(JsonOptions);
            if (!string.IsNullOrWhiteSpace(payload?.Message) && payload.Message != "Error")
            {
                return payload.Message;
            }
        }
        catch
        {
        }

        return fallback;
    }

    public async Task<(bool Ok, string Message)> PostMessageAsync(string url, object? body)
    {
        using var response = await _http.PostAsJsonAsync(url, body, JsonOptions);
        var payload = default(MessageModel);
        try
        {
            payload = await response.Content.ReadFromJsonAsync<MessageModel>(JsonOptions);
        }
        catch
        {
        }

        var message = string.IsNullOrWhiteSpace(payload?.Message)
            ? $"Error {(int)response.StatusCode}"
            : payload!.Message;

        return (response.IsSuccessStatusCode, message);
    }

    public async Task<T?> PutAsync<T>(string url, object? body)
    {
        using var response = await _http.PutAsJsonAsync(url, body, JsonOptions);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    public async Task<T?> DeleteAsync<T>(string url)
    {
        using var response = await _http.DeleteAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<T>(JsonOptions);
    }

    public async Task<(T? Data, string Error)> PostFileResultAsync<T>(string url, MultipartFormDataContent content)
    {
        using var response = await _http.PostAsync(url, content);
        if (!response.IsSuccessStatusCode)
        {
            return (default, await ReadErrorAsync(response, "Could not upload file"));
        }

        var data = await response.Content.ReadFromJsonAsync<T>(JsonOptions);
        return (data, "");
    }

    public async Task<Stream?> PostStreamAsync(string url, object body)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(body, options: JsonOptions)
        };
        var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadAsStreamAsync();
    }
}
