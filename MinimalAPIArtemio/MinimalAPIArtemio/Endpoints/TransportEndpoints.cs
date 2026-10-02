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

public static class TransportEndpoints
{
    public static void MapTransportEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/transports", async () =>
{
    try
    {
        var cDb = new ClaseDB<TransportModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Transports ORDER BY Transport;");

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
app.MapGet("/api/transports/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TransportModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Transports WHERE TransportId = {id};");

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
app.MapPost("/api/transports", async (TransportModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TransportModel>();
        data.TransportId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Transports (Transport) VALUES (@Transport);");

        return Results.Created($"/transports", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/transports", async (TransportModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TransportModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Transports SET Transport=@Transport WHERE TransportId=@TransportId;");

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
app.MapDelete("/api/transports/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TransportModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Transports WHERE TransportId={id};");

        return new
        {
            Message = "Transport deleted"
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
