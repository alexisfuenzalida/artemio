using ArtemioBlazorWA.Shared;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ArtemioBlazorWA.Tests;

public class RedirectToLoginTests : TestContext
{
    [Fact]
    public void RedirectsAnonymousUserToLogin()
    {
        var navigation = Services.GetRequiredService<NavigationManager>();
        RenderComponent<RedirectToLogin>();
        Assert.EndsWith("/", navigation.Uri);
    }
}
