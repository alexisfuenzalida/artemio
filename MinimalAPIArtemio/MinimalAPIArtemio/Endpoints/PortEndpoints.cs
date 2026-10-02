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

public static class PortEndpoints
{
    public static void MapPortEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/ports", async () =>
{
    try
    {
        var cDb = new ClaseDB<PortModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Ports ORDER BY Port;");

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
app.MapGet("/api/ports/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<PortModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Ports WHERE PortId = {id};");

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
app.MapPost("/api/ports", async (PortModel data) =>
{
    try
    {
        var cDb = new ClaseDB<PortModel>();
        data.PortId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Ports (Port,CountryId,FreeDays) VALUES (@Port,@CountryId,@FreeDays);");

        return Results.Created($"/ports", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/ports", async (PortModel data) =>
{
    try
    {
        var cDb = new ClaseDB<PortModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Ports SET Port=@Port,CountryId=@CountryId,FreeDays=@FreeDays WHERE PortId=@PortId;");

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
app.MapDelete("/api/ports/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<PortModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Ports WHERE PortId={id};");

        return new
        {
            Message = "Port deleted"
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
