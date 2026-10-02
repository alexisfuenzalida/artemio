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

public static class ConsigneeEndpoints
{
    public static void MapConsigneeEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/consignees", async () =>
{
    try
    {
        var cDb = new ClaseDB<ConsigneeModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Consignees ORDER BY Consignee;");

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
app.MapGet("/api/consignees/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ConsigneeModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Consignees WHERE ConsigneeId = {id};");

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
app.MapPost("/api/consignees", async (ConsigneeModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ConsigneeModel>();
        data.ConsigneeId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Consignees (Consignee) VALUES (@Consignee);");

        return Results.Created($"/consignees", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/consignees", async (ConsigneeModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ConsigneeModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Consignees SET Consignee=@Consignee WHERE ConsigneeId=@ConsigneeId;");

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
app.MapDelete("/api/consignees/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ConsigneeModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Consignees WHERE ConsigneeId={id};");

        return new
        {
            Message = "Consignee deleted"
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
