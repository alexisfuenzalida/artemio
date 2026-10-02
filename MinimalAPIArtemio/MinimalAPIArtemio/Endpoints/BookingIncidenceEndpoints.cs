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

public static class BookingIncidenceEndpoints
{
    public static void MapBookingIncidenceEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/bookingincidencesgrid/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingIncidenceGrid>();
        var data = await cDb.GetAllAsync(@$"SELECT
	                                        b.BookingId,
	                                        b.`NroBooking`,
	                                        b.`NroRequest`,
	                                        bd.`BL`,
	                                        bd.`Container`,
	                                        con.`Consignee`,
	                                        n.`Notify`,
	                                        (SELECT COUNT(bi.Container) FROM BookingIncidences bi WHERE bi.`BookingId` = bd.`BookingId` AND bi.`Container` = bd.`Container`) AS Incidences
                                        FROM `BookingDetails` bd
                                        LEFT JOIN Booking b ON b.`BookingId` = bd.`BookingId`
                                        LEFT JOIN Consignees con ON con.`ConsigneeId` = bd.`ConsigneeId`
                                        LEFT JOIN Notifies n ON n.`NotifyId` = bd.`NotifyId`
                                        WHERE b.`SeasonId` = {id};");

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
app.MapGet("/api/bookingincidences/{id}/{container}", async (int id,string container) =>
{
    try
    {
        var cDb = new ClaseDB<BookingIncidenceModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM BookingIncidences WHERE BookingId = @id AND Container = @container;", new { id, container });

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
app.MapPost("/api/bookingincidences", async (BookingIncidenceModel data) =>
{
    try
    {
        var cDb = new ClaseDB<BookingIncidenceModel>();
        await cDb.ExcecuteQueryAsync(data, @"INSERT INTO BookingIncidences (Container,IncidenceTypeId,Date,Incidence,BookingId) VALUES (@Container,@IncidenceTypeId,@Date,@Incidence,@BookingId);");

        return Results.Created($"/bookingincidences", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/bookingincidences/{id}/{container}", async (int id,string container) =>
{
    try
    {
        var cDb = new ClaseDB<BookingModel>();
        await cDb.ExcecuteQueryAsync("DELETE FROM BookingIncidences WHERE BookingId=@id AND Container=@container;", new { id, container });

        return new
        {
            Message = "Booking Incidence deleted"
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
