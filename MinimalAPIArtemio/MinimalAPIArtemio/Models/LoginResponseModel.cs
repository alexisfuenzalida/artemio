namespace MinimalAPIArtemio.Models;

public class LoginResponseModel
{
    public string Token { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}
