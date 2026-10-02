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

public static class InvolvedEndpoints
{
    public static void MapInvolvedEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/involved", async () =>
{
    try
    {
        var cDb = new ClaseDB<InvolvedModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Involved ORDER BY Involved;");

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
app.MapGet("/api/involved/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<InvolvedModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Involved WHERE InvolvedId = {id};");

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
app.MapPost("/api/involved", async (InvolvedModel data) =>
{
    try
    {
        var cDb = new ClaseDB<InvolvedModel>();
        data.InvolvedId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Involved (Involved) VALUES (@Involved);");

        return Results.Created($"/involved", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/involved", async (InvolvedModel data) =>
{
    try
    {
        var cDb = new ClaseDB<InvolvedModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Involved SET Involved=@Involved WHERE InvolvedId=@InvolvedId;");

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
app.MapDelete("/api/involved/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<InvolvedModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Involved WHERE InvolvedId={id};");

        return new
        {
            Message = "Involved deleted"
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
