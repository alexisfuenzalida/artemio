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

public static class CostItemEndpoints
{
    public static void MapCostItemEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/costitems", async () =>
{
    try
    {
        var cDb = new ClaseDB<CostItemModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM CostItems ORDER BY CostItem;");

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
app.MapGet("/api/costitems/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<CostItemModel>();
        var data = await cDb.GetAsync($"SELECT * FROM CostItems WHERE CostItemId = {id};");

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
app.MapPost("/api/costitems", async (CostItemModel data) =>
{
    try
    {
        var cDb = new ClaseDB<CostItemModel>();
        data.CostItemId = await cDb.ExecuteInsertAsync(data, "INSERT INTO CostItems (CostItem,GroupItem) VALUES (@CostItem,@GroupItem);");

        return Results.Created($"/costitems", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/costitems", async (CostItemModel data) =>
{
    try
    {
        var cDb = new ClaseDB<CostItemModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE CostItems SET CostItem=@CostItem,GroupItem=@GroupItem WHERE CostItemId=@CostItemId;");

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
app.MapDelete("/api/costitems/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<CostItemModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM CostItems WHERE CostItemId={id};");

        return new
        {
            Message = "Cost Item deleted"
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
