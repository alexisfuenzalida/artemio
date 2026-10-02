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

public static class BookingDetailEndpoints
{
    public static void MapBookingDetailEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/bookingdetails", async () =>
{
    try
    {
        var cDb = new ClaseDB<BookingDetailModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM BookingDetails;");

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
app.MapGet("/api/bookingdetails/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingDetailModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM BookingDetails WHERE BookingId = {id};");

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
app.MapPost("/api/bookingdetails", async (BookingDetailModel data) =>
{
    try
    {
        var cDb = new ClaseDB<BookingDetailModel>();
        await cDb.ExcecuteQueryAsync(data, @"INSERT INTO BookingDetails (BL,Container,ConsigneeId,NotifyId,Cases,StatusContainerId,Comment,BookingId,Comm,Rebate,Fee,RebateExp,SendFreeDays,SendFreeDaysComment) VALUES (@BL,@Container,@ConsigneeId,@NotifyId,@Cases,@StatusContainerId,@Comment,@BookingId,@Comm,@Rebate,@Fee,@RebateExp,@SendFreeDays,@SendFreeDaysComment);");

        return Results.Created($"/bookingdetails", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/bookingdetails/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM BookingDetails WHERE BookingId={id};");

        return new
        {
            Message = "Booking deleted"
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
