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

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/services", async () =>
{
    try
    {
        var cDb = new ClaseDB<ServiceModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Services ORDER BY Service;");

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
app.MapGet("/api/services/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ServiceModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Services WHERE ServiceId = {id};");

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
app.MapPost("/api/services", async (ServiceModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ServiceModel>();
        data.ServiceId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Services (Service) VALUES (@Service);");

        return Results.Created($"/servicies", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/services", async (ServiceModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ServiceModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Services SET Service=@Service WHERE ServiceId=@ServiceId;");

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
app.MapDelete("/api/services/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ServiceModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Services WHERE ServiceId={id};");

        return new
        {
            Message = "Season deleted"
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
