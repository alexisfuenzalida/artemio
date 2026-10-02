namespace ArtemioBlazorWA.Auth;

public class AuthTokenStore
{
    private int _handlingUnauthorized;

    public string? Token { get; set; }

    public event Func<Task>? Unauthorized;

    public bool TryBeginUnauthorizedHandling() =>
        Interlocked.CompareExchange(ref _handlingUnauthorized, 1, 0) == 0;

    public void ResetUnauthorizedHandling() =>
        Interlocked.Exchange(ref _handlingUnauthorized, 0);

    public void NotifyUnauthorized()
    {
        if (!TryBeginUnauthorizedHandling())
        {
            return;
        }

        Token = null;
        var handler = Unauthorized;
        if (handler is null)
        {
            return;
        }

        _ = handler();
    }
}
