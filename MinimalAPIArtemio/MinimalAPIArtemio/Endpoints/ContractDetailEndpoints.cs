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

public static class ContractDetailEndpoints
{
    public static void MapContractDetailEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/contractsdetails", async () =>
{
    try
    {
        var cDb = new ClaseDB<ContractDetailModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM ContractsDetails;");

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
app.MapGet("/api/contractsdetails/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ContractDetailModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM ContractsDetails WHERE ContractId = {id};");

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
app.MapPost("/api/contractsdetails", async (ContractDetailModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ContractDetailModel>();
        await cDb.ExcecuteQueryAsync(data, @"INSERT INTO ContractsDetails (CostItemId,Amount,PaymentConditionId,Include,ContractId) VALUES (@CostItemId,@Amount,@PaymentConditionId,@Include,@ContractId);");

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
app.MapDelete("/api/contractsdetails/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ContractModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM ContractsDetails WHERE ContractId={id};");

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
