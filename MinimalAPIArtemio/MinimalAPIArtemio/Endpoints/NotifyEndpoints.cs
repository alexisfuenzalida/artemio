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

public static class NotifyEndpoints
{
    public static void MapNotifyEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/notifies", async () =>
{
    try
    {
        var cDb = new ClaseDB<NotifyModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Notifies ORDER BY Notify;");

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
app.MapGet("/api/notifies/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<NotifyModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Notifies WHERE NotifyId = {id};");

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
app.MapPost("/api/notifies", async (NotifyModel data) =>
{
    try
    {
        var cDb = new ClaseDB<NotifyModel>();
        data.NotifyId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Notifies (Notify) VALUES (@Notify);");

        return Results.Created($"/notifies", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/notifies", async (NotifyModel data) =>
{
    try
    {
        var cDb = new ClaseDB<NotifyModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Notifies SET Notify=@Notify WHERE NotifyId=@NotifyId;");

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
app.MapDelete("/api/notifies/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<NotifyModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Notifies WHERE NotifyId={id};");

        return new
        {
            Message = "Notify deleted"
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
