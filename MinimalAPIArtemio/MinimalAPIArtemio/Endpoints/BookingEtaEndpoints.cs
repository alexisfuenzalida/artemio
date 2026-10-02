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

public static class BookingEtaEndpoints
{
    public static void MapBookingEtaEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/bookingeta/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingETAModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM BookingETA WHERE BookingId = {id};");

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
app.MapPost("/api/bookingeta", async (BookingETAModel data) =>
{
    try
    {
        var cDb = new ClaseDB<BookingETAModel>();
        await cDb.ExcecuteQueryAsync(data, @"INSERT INTO BookingETA (Nro,ETA,BookingId) VALUES (@Nro,@ETA,@BookingId);");

        return Results.Created($"/bookingeta", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/bookingeta/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingETAModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM BookingETA WHERE BookingId={id};");

        return new
        {
            Message = "Booking ETA deleted"
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
