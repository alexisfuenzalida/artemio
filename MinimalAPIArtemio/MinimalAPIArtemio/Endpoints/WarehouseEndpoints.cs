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

public static class WarehouseEndpoints
{
    public static void MapWarehouseEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/warehouses", async () =>
{
    try
    {
        var cDb = new ClaseDB<WarehouseModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Warehouses ORDER BY Warehouse;");

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
app.MapGet("/api/warehouses/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<WarehouseModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Warehouses WHERE WarehouseId = {id};");

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
app.MapPost("/api/warehouses", async (WarehouseModel data) =>
{
    try
    {
        var cDb = new ClaseDB<WarehouseModel>();
        data.WarehouseId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Warehouses (Warehouse,CityId) VALUES (@Warehouse,@CityId);");

        return Results.Created($"/warehouses", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/warehouses", async (WarehouseModel data) =>
{
    try
    {
        var cDb = new ClaseDB<WarehouseModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Warehouses SET Warehouse=@Warehouse,CityId=@CityId WHERE WarehouseId=@WarehouseId;");

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
app.MapDelete("/api/warehouses/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<WarehouseModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Warehouses WHERE WarehouseId={id};");

        return new
        {
            Message = "Warehouse deleted"
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
