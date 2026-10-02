using Blazored.LocalStorage;

namespace ArtemioBlazorWA.Tests;

internal sealed class NullLocalStorage : ILocalStorageService
{
    public event EventHandler<ChangingEventArgs>? Changing
    {
        add { }
        remove { }
    }

    public event EventHandler<ChangedEventArgs>? Changed
    {
        add { }
        remove { }
    }

    public ValueTask ClearAsync(CancellationToken cancellationToken = default) => ValueTask.CompletedTask;

    public ValueTask<bool> ContainKeyAsync(string key, CancellationToken cancellationToken = default) => ValueTask.FromResult(false);

    public ValueTask<string?> KeyAsync(int index, CancellationToken cancellationToken = default) => ValueTask.FromResult<string?>(null);

    public ValueTask<IEnumerable<string>> KeysAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromResult<IEnumerable<string>>(Array.Empty<string>());

    public ValueTask<int> LengthAsync(CancellationToken cancellationToken = default) => ValueTask.FromResult(0);

    public ValueTask RemoveItemAsync(string key, CancellationToken cancellationToken = default) => ValueTask.CompletedTask;

    public ValueTask RemoveItemsAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default) => ValueTask.CompletedTask;

    public ValueTask SetItemAsync<T>(string key, T data, CancellationToken cancellationToken = default) => ValueTask.CompletedTask;

    public ValueTask SetItemAsStringAsync(string key, string data, CancellationToken cancellationToken = default) => ValueTask.CompletedTask;

    public ValueTask<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken = default) => ValueTask.FromResult<T?>(default);

    public ValueTask<string?> GetItemAsStringAsync(string key, CancellationToken cancellationToken = default) => ValueTask.FromResult<string?>(null);
}
