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

public static class StackingEndpoints
{
    public static void MapStackingEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/stacking", async () =>
{
    try
    {
        var cDb = new ClaseDB<StackingModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Stacking ORDER BY StackingId;");

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
app.MapGet("/api/stacking/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<StackingModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Stacking WHERE StackingId = {id};");

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
app.MapGet("/api/stackingbyservice/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<StackingModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Stacking WHERE ServiceId = {id};");

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
app.MapGet("/api/stacking/{tc}/{se}", async (int tc,int se) =>
{
    try
    {
        var cDb = new ClaseDB<StackingModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Stacking WHERE TransportCompanyId = {tc} AND ServiceId = {se};");

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
app.MapPost("/api/stacking", async (StackingModel data) =>
{
    try
    {
        var cDb = new ClaseDB<StackingModel>();
        data.StackingId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Stacking (Week,ServiceId,Stacking,LateArrival,ExtraLateArrival,CutOff) VALUES (@Week,@ServiceId,@Stacking,@LateArrival,@ExtraLateArrival,@CutOff);");

        return Results.Created($"/stacking", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/stacking", async (StackingModel data) =>
{
    try
    {
        var cDb = new ClaseDB<StackingModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Stacking SET Week=@Week,ServiceId=@ServiceId,Stacking=@Stacking,LateArrival=@LateArrival,ExtraLateArrival=@ExtraLateArrival,CutOff=@CutOff WHERE StackingId=@StackingId;");

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
app.MapDelete("/api/stacking/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<StackingModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Stacking WHERE StackingId={id};");

        return new
        {
            Message = "Stacking deleted"
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
