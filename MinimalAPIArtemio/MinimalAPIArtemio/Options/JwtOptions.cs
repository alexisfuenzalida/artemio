namespace MinimalAPIArtemio.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = "MinimalAPIArtemio";
    public string Audience { get; set; } = "ArtemioBlazorWA";
    public int ExpiresMinutes { get; set; } = 480;
}
