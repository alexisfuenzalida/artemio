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

public static class ContainerTypeEndpoints
{
    public static void MapContainerTypeEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/containerstype", async () =>
{
    try
    {
        var cDb = new ClaseDB<ContainerTypeModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM ContainersType ORDER BY ContainerType;");

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
app.MapGet("/api/containerstype/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ContainerTypeModel>();
        var data = await cDb.GetAsync($"SELECT * FROM ContainersType WHERE ContainerTypeId = {id};");

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
app.MapPost("/api/containerstype", async (ContainerTypeModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ContainerTypeModel>();
        data.ContainerTypeId = await cDb.ExecuteInsertAsync(data, "INSERT INTO ContainersType (ContainerType) VALUES (@ContainerType);");

        return Results.Created($"/containerstype", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/containerstype", async (ContainerTypeModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ContainerTypeModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE ContainersType SET ContainerType=@ContainerType WHERE ContainerTypeId=@ContainerTypeId;");

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
app.MapDelete("/api/containerstype/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ContainerTypeModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM ContainersType WHERE ContainerTypeId={id};");

        return new
        {
            Message = "Container Type deleted"
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
