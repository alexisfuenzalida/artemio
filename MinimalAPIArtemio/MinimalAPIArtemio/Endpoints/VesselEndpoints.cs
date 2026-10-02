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

public static class VesselEndpoints
{
    public static void MapVesselEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/vessels", async () =>
{
    try
    {
        var cDb = new ClaseDB<VesselModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Vessels ORDER BY Vessel;");

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
app.MapGet("/api/vessels/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<VesselModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Vessels WHERE VesselId = {id};");

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
app.MapPost("/api/vessels", async (VesselModel data) =>
{
    try
    {
        var cDb = new ClaseDB<VesselModel>();
        data.VesselId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Vessels (Vessel) VALUES (@Vessel);");

        return Results.Created($"/vessels", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/vessels", async (VesselModel data) =>
{
    try
    {
        var cDb = new ClaseDB<VesselModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Vessels SET Vessel=@Vessel WHERE VesselId=@VesselId;");

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
app.MapDelete("/api/vessels/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<VesselModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Vessels WHERE VesselId={id};");

        return new
        {
            Message = "Vessel deleted"
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
