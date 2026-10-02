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

public static class IncidenceTypeEndpoints
{
    public static void MapIncidenceTypeEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/incidencestype", async () =>
{
    try
    {
        var cDb = new ClaseDB<IncidenceTypeModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM IncidencesType ORDER BY IncidenceType;");

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
app.MapGet("/api/incidencestype/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<IncidenceTypeModel>();
        var data = await cDb.GetAsync($"SELECT * FROM IncidencesType WHERE IncidenceTypeId = {id};");

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
app.MapPost("/api/incidencestype", async (IncidenceTypeModel data) =>
{
    try
    {
        var cDb = new ClaseDB<IncidenceTypeModel>();
        data.IncidenceTypeId = await cDb.ExecuteInsertAsync(data, "INSERT INTO IncidencesType (IncidenceType) VALUES (@IncidenceType);");

        return Results.Created($"/incidencestype", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/incidencestype", async (IncidenceTypeModel data) =>
{
    try
    {
        var cDb = new ClaseDB<IncidenceTypeModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE IncidencesType SET IncidenceType=@IncidenceType WHERE IncidenceTypeId=@IncidenceTypeId;");

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
app.MapDelete("/api/incidencestype/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<IncidenceTypeModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM IncidencesType WHERE IncidenceTypeId={id};");

        return new
        {
            Message = "Country deleted"
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
