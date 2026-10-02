using System.ComponentModel.DataAnnotations;
using MinimalAPIArtemio.Auth;
using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;

namespace MinimalAPIArtemio.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/login", async (LoginModel login, JwtTokenService tokens) =>
        {
            if (string.IsNullOrWhiteSpace(login.User) || string.IsNullOrWhiteSpace(login.Password))
            {
                return Results.BadRequest(new { Message = "User and password are required" });
            }

            var cLogin = new ClaseDB<UserModel>();
            var em = await cLogin.GetAsync("SELECT * FROM Users WHERE User = @User;", new { login.User });

            if (em is null || string.IsNullOrEmpty(em.Password) || !BCrypt.Net.BCrypt.Verify(login.Password, em.Password))
            {
                return Results.BadRequest(new { Message = "User or password wrong" });
            }

            var (token, expires) = tokens.CreateToken(em);
            return Results.Ok(new LoginResponseModel
            {
                Token = token,
                User = em.User,
                Email = em.Email,
                Type = em.Type,
                Expires = expires
            });
        })
        .AllowAnonymous()
        .RequireRateLimiting("login")
        .WithTags("Auth");
    }
}
