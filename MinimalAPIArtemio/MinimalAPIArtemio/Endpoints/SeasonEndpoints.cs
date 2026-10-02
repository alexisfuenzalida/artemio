using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Helpers;
using MinimalAPIArtemio.Models;
using MinimalAPIArtemio.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MinimalAPIArtemio.Endpoints;

public static class SeasonEndpoints
{
    public static void MapSeasonEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/seasons", async () =>
{
    try
    {
        var cDb = new ClaseDB<SeasonModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Seasons ORDER BY Season;");

        return Results.Ok(data);
    }
    catch (Exception)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
}).AllowAnonymous().WithTags("Seasons");
app.MapGet("/api/seasons/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<SeasonModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Seasons WHERE SeasonId = {id};");

        if (data == null)
        {
            return Results.NotFound(new
            {
                Message = "Not found"
            });
        }
        else
        {
            return Results.Ok(data);
        }
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex.Message);
    }
});
app.MapPost("/api/seasons", async (SeasonModel data) =>
{
    try
    {
        var cDb = new ClaseDB<SeasonModel>();
        data.SeasonId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Seasons (Season) VALUES (@Season);");

        return Results.Created($"/seasons", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/seasons", async (SeasonModel data) =>
{
    try
    {
        var cDb = new ClaseDB<SeasonModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Seasons SET Season=@Season WHERE SeasonId=@SeasonId;");

        return Results.Ok(data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/seasons/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<SeasonModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Seasons WHERE SeasonId={id};");

        return new
        {
            Message = "Season deleted"
        };
    }
    catch (Exception ex)
    {
        return new
        {
            Message = "Error"
        };
    }
});
    }
}
