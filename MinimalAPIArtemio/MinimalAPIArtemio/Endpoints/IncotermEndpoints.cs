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

public static class IncotermEndpoints
{
    public static void MapIncotermEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/incoterms", async () =>
{
    try
    {
        var cDb = new ClaseDB<IncotermModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Incoterms ORDER BY Incoterm;");

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
app.MapGet("/api/incoterms/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<IncotermModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Incoterms WHERE IncotermId = {id};");

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
app.MapPost("/api/incoterms", async (IncotermModel data) =>
{
    try
    {
        var cDb = new ClaseDB<IncotermModel>();
        data.IncotermId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Incoterms (Incoterm) VALUES (@Incoterm);");

        return Results.Created($"/incoterms", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/incoterms", async (IncotermModel data) =>
{
    try
    {
        var cDb = new ClaseDB<IncotermModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Incoterms SET Incoterm=@Incoterm WHERE IncotermId=@IncotermId;");

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
app.MapDelete("/api/incoterms/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<IncotermModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Incoterms WHERE IncotermId={id};");

        return new
        {
            Message = "Incoterm deleted"
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
