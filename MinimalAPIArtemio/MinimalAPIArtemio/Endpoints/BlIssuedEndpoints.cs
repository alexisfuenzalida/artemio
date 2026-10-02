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

public static class BlIssuedEndpoints
{
    public static void MapBlIssuedEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/blissueds", async () =>
{
    try
    {
        var cDb = new ClaseDB<BlIssuedModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM BlIssueds ORDER BY BlIssued;");

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
app.MapGet("/api/blissueds/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BlIssuedModel>();
        var data = await cDb.GetAsync($"SELECT * FROM BlIssueds WHERE BlIssuedId = {id};");

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
app.MapPost("/api/blissueds", async (BlIssuedModel data) =>
{
    try
    {
        var cDb = new ClaseDB<BlIssuedModel>();
        data.BlIssuedId = await cDb.ExecuteInsertAsync(data, "INSERT INTO BlIssueds (BlIssued) VALUES (@BlIssued);");

        return Results.Created($"/blissueds", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/blissueds", async (BlIssuedModel data) =>
{
    try
    {
        var cDb = new ClaseDB<BlIssuedModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE BlIssueds SET BlIssued=@BlIssued WHERE BlIssuedId=@BlIssuedId;");

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
app.MapDelete("/api/blissueds/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BlIssuedModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM BlIssueds WHERE BlIssuedId={id};");

        return new
        {
            Message = "Bl Issued deleted"
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
