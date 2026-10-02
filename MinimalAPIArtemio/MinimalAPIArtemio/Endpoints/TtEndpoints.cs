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

public static class TtEndpoints
{
    public static void MapTtEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/ttssearch/{serviceId}/{polId}", async (int serviceId, int polId) =>
{    
    try
    {
        var cDb = new ClaseDB<TtDetailModel>();
        var data = await cDb.GetAllAsync($"select td.* from TtsDetails td left join TTs t on t.`Id` = td.`Id` where t.`POLId` = {polId} and t.`ServiceId` = {serviceId} and CURDATE() between t._From AND t.`_To`;");

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

app.MapGet("/api/tts", async () =>
{
    try
    {
        var cDb = new ClaseDB<TtModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM TTs ORDER BY Id DESC;");

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
app.MapGet("/api/tts/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TtModel>();
        var data = await cDb.GetAsync($"SELECT * FROM TTs WHERE Id = {id};");

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
app.MapPost("/api/tts", async (TtModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TtModel>();
        data.Id = await cDb.ExecuteInsertAsync(data, "INSERT INTO TTs (ServiceId,POLId,_From,_To) VALUES (@ServiceId,@POLId,@_From,@_To);");

        return Results.Created($"/tts", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/tts", async (TtModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TtModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE TTs SET ServiceId=@ServiceId,POLId=@POLId,_From=@_From,_To=@_To WHERE Id=@Id;");

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
app.MapDelete("/api/tts/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TtModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM TTs WHERE Id={id};");

        return new
        {
            Message = "Tt deleted"
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
