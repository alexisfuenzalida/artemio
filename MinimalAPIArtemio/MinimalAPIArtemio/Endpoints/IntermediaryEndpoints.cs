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

public static class IntermediaryEndpoints
{
    public static void MapIntermediaryEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/intermediaries", async () =>
{
    try
    {
        var cDb = new ClaseDB<IntermediaryModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Intermediaries ORDER BY Intermediary;");

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
app.MapGet("/api/intermediaries/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<IntermediaryModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Intermediaries WHERE IntermediaryId = {id};");

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
app.MapPost("/api/intermediaries", async (IntermediaryModel data) =>
{
    try
    {
        var cDb = new ClaseDB<IntermediaryModel>();
        data.IntermediaryId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Intermediaries (Intermediary) VALUES (@Intermediary);");

        return Results.Created($"/intermediaries", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/intermediaries", async (IntermediaryModel data) =>
{
    try
    {
        var cDb = new ClaseDB<IntermediaryModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Intermediaries SET Intermediary=@Intermediary WHERE IntermediaryId=@IntermediaryId;");

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
app.MapDelete("/api/intermediaries/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<CountryModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Intermediaries WHERE IntermediaryId={id};");

        return new
        {
            Message = "Intermediry deleted"
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
