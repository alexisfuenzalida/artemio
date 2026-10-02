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

public static class FreeDaysEndpoints
{
    public static void MapFreeDaysEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/freedays", async () =>
{
    try
    {
        var cDb = new ClaseDB<FreeDaysModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM FreeDays ORDER BY Id;");

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
app.MapGet("/api/freedays/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<FreeDaysModel>();
        var data = await cDb.GetAsync($"SELECT * FROM FreeDays WHERE Id = {id};");

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
app.MapPost("/api/freedays", async (FreeDaysModel data) =>
{
    try
    {
        var cDb = new ClaseDB<FreeDaysModel>();
        data.Id = await cDb.ExecuteInsertAsync(data, "INSERT INTO FreeDays (TransportCompanyId,CountryId,FreeDays) VALUES (@TransportCompanyId,@CountryId,@FreeDays);");

        return Results.Created($"/freedays", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/freedays", async (FreeDaysModel data) =>
{
    try
    {
        var cDb = new ClaseDB<FreeDaysModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE FreeDays SET TransportCompanyId=@TransportCompanyId,CountryId=@CountryId,FreeDays=@FreeDays WHERE Id=@Id;");

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
app.MapDelete("/api/freedays/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<FreeDaysModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM FreeDays WHERE Id={id};");

        return new
        {
            Message = "FreeDays deleted"
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
