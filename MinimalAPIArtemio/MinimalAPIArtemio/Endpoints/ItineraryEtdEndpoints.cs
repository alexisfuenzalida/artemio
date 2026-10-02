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

public static class ItineraryEtdEndpoints
{
    public static void MapItineraryEtdEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/itinerariesetd", async () =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryETDModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM ItinerariesETD;");

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
app.MapGet("/api/itinerariesetd/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryETDModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM ItinerariesETD WHERE ItineraryId = {id};");

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
app.MapPost("/api/itinerariesetd", async (ItineraryETDModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryETDModel>();
        await cDb.ExcecuteQueryAsync(data, "INSERT INTO ItinerariesETD (POLId,ETD,IsMain,ItineraryId) VALUES (@POLId,@ETD,@IsMain,@ItineraryId);");

        return Results.Created($"/itinerariesetd", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/itinerariesetd/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryETDModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM ItinerariesETD WHERE ItineraryId={id};");

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
