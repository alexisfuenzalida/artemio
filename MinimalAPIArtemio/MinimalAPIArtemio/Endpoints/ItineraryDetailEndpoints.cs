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

public static class ItineraryDetailEndpoints
{
    public static void MapItineraryDetailEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/itinerariesdetails", async () =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryDetailModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM ItinerariesDetails;");

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
app.MapGet("/api/itinerariesdetails/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryDetailModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM ItinerariesDetails WHERE ItineraryId = {id};");

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
app.MapPost("/api/itinerariesdetails", async (ItineraryDetailModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryDetailModel>();
        await cDb.ExcecuteQueryAsync(data, "INSERT INTO ItinerariesDetails (PODId,ETA,TT,ItineraryId) VALUES (@PODId,@ETA,@TT,@ItineraryId);");

        return Results.Created($"/itinerariesdetails", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/itinerariesdetails/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryDetailModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM ItinerariesDetails WHERE ItineraryId={id};");

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
