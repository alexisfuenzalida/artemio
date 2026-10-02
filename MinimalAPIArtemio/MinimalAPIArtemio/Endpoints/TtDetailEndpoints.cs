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

public static class TtDetailEndpoints
{
    public static void MapTtDetailEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/ttsdetails/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TtDetailModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM TtsDetails WHERE Id = {id};");

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
app.MapPost("/api/ttsdetails", async (TtDetailModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TtDetailModel>();
        await cDb.ExcecuteQueryAsync(data, "INSERT INTO TtsDetails (PODId,TT,Id) VALUES (@PODId,@TT,@Id);");

        return Results.Created($"/ttsdetails", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/ttsdetails/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TtDetailModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM TtsDetails WHERE Id={id};");

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
