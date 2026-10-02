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

public static class ContractTypeEndpoints
{
    public static void MapContractTypeEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/contracttypes", async () =>
{
    try
    {
        var cDb = new ClaseDB<ContractTypeModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM ContractTypes ORDER BY ContractType;");

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
app.MapGet("/api/contracttypes/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ContractTypeModel>();
        var data = await cDb.GetAsync($"SELECT * FROM ContractTypes WHERE ContractTypeId = {id};");

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
app.MapPost("/api/contracttypes", async (ContractTypeModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ContractTypeModel>();
        data.ContractTypeId = await cDb.ExecuteInsertAsync(data, "INSERT INTO ContractTypes (ContractType) VALUES (@ContractType);");

        return Results.Created($"/contracttypes", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/contracttypes", async (ContractTypeModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ContractTypeModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE ContractTypes SET ContractType=@ContractType WHERE ContractTypeId=@ContractTypeId;");

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
app.MapDelete("/api/contracttypes/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ContractTypeModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM ContractTypes WHERE ContractTypeId={id};");

        return new
        {
            Message = "Contract Type deleted"
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
