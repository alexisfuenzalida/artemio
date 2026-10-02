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

public static class BookingPodEndpoints
{
    public static void MapBookingPodEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/bookingpod/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingPODModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM BookingPOD WHERE BookingId = {id};");

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
app.MapPost("/api/bookingpod", async (BookingPODModel data) =>
{
    try
    {
        var cDb = new ClaseDB<BookingPODModel>();
        await cDb.ExcecuteQueryAsync(data, @"INSERT INTO BookingPOD (Nro,POD,BookingId) VALUES (@Nro,@POD,@BookingId);");

        return Results.Created($"/bookingpod", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/bookingpod/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingPODModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM BookingPOD WHERE BookingId={id};");

        return new
        {
            Message = "Booking POD deleted"
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
