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

public static class CommodityEndpoints
{
    public static void MapCommodityEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/commodities", async () =>
{
    try
    {
        var cDb = new ClaseDB<CommodityModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Commodities ORDER BY Commodity;");

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
app.MapGet("/api/commodities/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<CommodityModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Commodities WHERE CommodityId = {id};");

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
app.MapPost("/api/commodities", async (CommodityModel data) =>
{
    try
    {
        var cDb = new ClaseDB<CommodityModel>();
        data.CommodityId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Commodities (Code,Commodity) VALUES (@Code,@Commodity);");

        return Results.Created($"/commodities", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/commodities", async (CommodityModel data) =>
{
    try
    {
        var cDb = new ClaseDB<CommodityModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Commodities SET Code=@Code,Commodity=@Commodity WHERE CommodityId=@CommodityId;");

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
app.MapDelete("/api/commodities/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<CommodityModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Commodities WHERE CommodityId={id};");

        return new
        {
            Message = "Commodity deleted"
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
