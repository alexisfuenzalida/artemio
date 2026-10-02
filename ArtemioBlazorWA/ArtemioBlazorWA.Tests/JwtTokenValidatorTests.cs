using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ArtemioBlazorWA.Auth;
using Xunit;

namespace ArtemioBlazorWA.Tests;

public class JwtTokenValidatorTests
{
    [Fact]
    public void RejectsNullOrEmptyToken()
    {
        Assert.False(JwtTokenValidator.TryGetClaims(null, out _));
        Assert.False(JwtTokenValidator.TryGetClaims("", out _));
        Assert.False(JwtTokenValidator.TryGetClaims("not-a-jwt", out _));
    }

    [Fact]
    public void RejectsExpiredToken()
    {
        var token = CreateToken(DateTime.UtcNow.AddMinutes(-5));
        Assert.False(JwtTokenValidator.TryGetClaims(token, out _));
    }

    [Fact]
    public void AcceptsValidToken()
    {
        var token = CreateToken(DateTime.UtcNow.AddHours(1));
        Assert.True(JwtTokenValidator.TryGetClaims(token, out var claims));
        Assert.Contains(claims, c => c.Type == "sub" && c.Value == "artemio");
    }

    private static string CreateToken(DateTime expires)
    {
        var jwt = new JwtSecurityToken(
            claims: new[] { new Claim("sub", "artemio") },
            expires: expires);
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
