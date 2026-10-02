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

public static class StatusContainerEndpoints
{
    public static void MapStatusContainerEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/statuscontainers", async () =>
{
    try
    {
        var cDb = new ClaseDB<StatusContainerModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM StatusContainers ORDER BY StatusContainer;");

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
app.MapGet("/api/statuscontainers/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<StatusContainerModel>();
        var data = await cDb.GetAsync($"SELECT * FROM StatusContainers WHERE StatusContainerId = {id};");

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
app.MapPost("/api/statuscontainers", async (StatusContainerModel data) =>
{
    try
    {
        var cDb = new ClaseDB<StatusContainerModel>();
        data.StatusContainerId = await cDb.ExecuteInsertAsync(data, "INSERT INTO StatusContainers (StatusContainer) VALUES (@StatusContainer);");

        return Results.Created($"/statuscontainers", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/statuscontainers", async (StatusContainerModel data) =>
{
    try
    {
        var cDb = new ClaseDB<StatusContainerModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE StatusContainers SET StatusContainer=@StatusContainer WHERE StatusContainerId=@StatusContainerId;");

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
app.MapDelete("/api/statuscontainers/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<StatusContainerModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM StatusContainers WHERE StatusContainerId={id};");

        return new
        {
            Message = "StatusContainer deleted"
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
