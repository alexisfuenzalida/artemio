using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ArtemioBlazorWA.Auth;

public static class JwtTokenValidator
{
    public static bool TryGetClaims(string? token, out IReadOnlyList<Claim> claims)
    {
        claims = Array.Empty<Claim>();
        if (string.IsNullOrWhiteSpace(token))
        {
            return false;
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
            {
                return false;
            }

            var jwt = handler.ReadJwtToken(token);
            if (jwt.ValidTo != DateTime.MinValue && jwt.ValidTo <= DateTime.UtcNow)
            {
                return false;
            }

            claims = jwt.Claims.ToList();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
