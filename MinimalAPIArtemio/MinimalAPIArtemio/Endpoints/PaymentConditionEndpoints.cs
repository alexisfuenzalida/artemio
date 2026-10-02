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

public static class PaymentConditionEndpoints
{
    public static void MapPaymentConditionEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/paymentconditions", async () =>
{
    try
    {
        var cDb = new ClaseDB<PaymentConditionModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM PaymentConditions ORDER BY PaymentCondition;");

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
app.MapGet("/api/paymentconditions/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<PaymentConditionModel>();
        var data = await cDb.GetAsync($"SELECT * FROM PaymentConditions WHERE PaymentConditionId = {id};");

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
app.MapPost("/api/paymentconditions", async (PaymentConditionModel data) =>
{
    try
    {
        var cDb = new ClaseDB<PaymentConditionModel>();
        data.PaymentConditionId = await cDb.ExecuteInsertAsync(data, "INSERT INTO PaymentConditions (PaymentCondition) VALUES (@PaymentCondition);");

        return Results.Created($"/paymentconditions", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/paymentconditions", async (PaymentConditionModel data) =>
{
    try
    {
        var cDb = new ClaseDB<PaymentConditionModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE PaymentConditions SET PaymentCondition=@PaymentCondition WHERE PaymentConditionId=@PaymentConditionId;");

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
app.MapDelete("/api/paymentconditions/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<PaymentConditionModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM PaymentConditions WHERE PaymentConditionId={id};");

        return new
        {
            Message = "Payment Condition deleted"
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
