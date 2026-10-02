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

public static class ContractConditionEndpoints
{
    public static void MapContractConditionEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/contractsconditions", async () =>
{
    try
    {
        var cDb = new ClaseDB<ContractConditionModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM ContractsConditions;");

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
app.MapGet("/api/contractsconditions/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ContractConditionModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM ContractsConditions WHERE ContractId = {id};");

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
app.MapPost("/api/contractsconditions", async (ContractConditionModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ContractConditionModel>();
        await cDb.ExcecuteQueryAsync(data, @"INSERT INTO ContractsConditions (ShipperId,IntermediaryId,Public,Net,FeeShipper,FeeIntermediary,FeeArtemio,Comm,ContractId) VALUES (@ShipperId,@IntermediaryId,@Public,@Net,@FeeShipper,@FeeIntermediary,@FeeArtemio,@Comm,@ContractId);");

        return Results.Created($"/contractsdetails", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/contractsconditions/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ContractConditionModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM ContractsConditions WHERE ContractId={id};");

        return new
        {
            Message = "Contract deleted"
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
