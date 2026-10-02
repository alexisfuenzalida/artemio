using System.ComponentModel.DataAnnotations;

namespace Artemio.Contracts;

public class LoginRequest
{
    [Required]
    public string User { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public int SeasonId { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}

public class UserDto
{
    public int UserId { get; set; }

    [Required]
    public string User { get; set; } = string.Empty;

    public string? Password { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public string? Type { get; set; }
}

public class MessageDto
{
    public string Message { get; set; } = string.Empty;
}
