using MinimalAPIArtemio.Database;
using MySql.Data.MySqlClient;

namespace MinimalAPIArtemio.Endpoints;

public static class HealthEndpoints
{
    public static void MapHealthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/health", async (IDbConnectionFactory factory) =>
        {
            try
            {
                await using var connection = new MySqlConnection(factory.ConnectionString);
                await connection.OpenAsync();
                return Results.Ok(new { status = "ok" });
            }
            catch (Exception)
            {
                return Results.Problem(title: "Database unavailable", statusCode: 503);
            }
        })
        .AllowAnonymous()
        .WithTags("Health");
    }
}
