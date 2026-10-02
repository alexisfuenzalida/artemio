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

public static class ItineraryTransportCompanyEndpoints
{
    public static void MapItineraryTransportCompanyEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/itinerariestransportcompanies", async () =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryTransportCompanyModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM ItinerariesTransportCompanies;");

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
app.MapGet("/api/itinerariestransportcompanies/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryTransportCompanyModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM ItinerariesTransportCompanies WHERE ItineraryId = {id};");

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
app.MapPost("/api/itinerariestransportcompanies", async (ItineraryTransportCompanyModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryTransportCompanyModel>();
        await cDb.ExcecuteQueryAsync(data, "INSERT INTO ItinerariesTransportCompanies (TransportCompanyId,ItineraryId) VALUES (@TransportCompanyId,@ItineraryId);");

        return Results.Created($"/itinerariestransportcompanies", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/itinerariestransportcompanies/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<CountryModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM ItinerariesTransportCompanies WHERE ItineraryId={id};");

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
