using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;

namespace MinimalAPIArtemio.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users").RequireAuthorization();

        group.MapGet("/", async () =>
        {
            var cUser = new ClaseDB<UserModel>();
            var u = await cUser.GetAllAsync("SELECT UserId,User,Email,Type FROM Users;");
            return Results.Ok(u);
        });

        group.MapGet("/{id:int}", async (int id) =>
        {
            var cUser = new ClaseDB<UserModel>();
            var u = await cUser.GetAsync("SELECT UserId,User,Email,Type FROM Users WHERE UserId = @id;", new { id });
            return u is null ? Results.NotFound(new { Message = "Not found" }) : Results.Ok(u);
        });

        group.MapPost("/", async (UserModel u) =>
        {
            if (string.IsNullOrWhiteSpace(u.User) || string.IsNullOrWhiteSpace(u.Password))
            {
                return Results.BadRequest(new { Message = "User and password are required" });
            }

            u.Password = BCrypt.Net.BCrypt.HashPassword(u.Password);
            var cUser = new ClaseDB<UserModel>();
            await cUser.ExcecuteQueryAsync(u, "INSERT INTO Users (User,Password,Email,Type) VALUES (@User,@Password,@Email,@Type);");
            return Results.Ok(new { Message = "User created" });
        });

        group.MapPut("/", async (UserModel u) =>
        {
            if (u.UserId <= 0)
            {
                return Results.BadRequest(new { Message = "UserId is required" });
            }

            var cUser = new ClaseDB<UserModel>();
            if (!string.IsNullOrWhiteSpace(u.Password))
            {
                u.Password = BCrypt.Net.BCrypt.HashPassword(u.Password);
                await cUser.ExcecuteQueryAsync(u, "UPDATE Users SET User=@User,Password=@Password,Email=@Email,Type=@Type WHERE UserId=@UserId;");
            }
            else
            {
                await cUser.ExcecuteQueryAsync(u, "UPDATE Users SET User=@User,Email=@Email,Type=@Type WHERE UserId=@UserId;");
            }

            return Results.Ok(new { Message = "User updated" });
        });

        group.MapDelete("/{id:int}", async (int id) =>
        {
            var cUser = new ClaseDB<UserModel>();
            await cUser.ExcecuteQueryAsync("DELETE FROM Users WHERE UserId=@UserId;", new { UserId = id });
            return Results.Ok(new { Message = "User deleted" });
        });
    }
}
