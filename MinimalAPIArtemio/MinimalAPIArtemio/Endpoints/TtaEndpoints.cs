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

public static class TtaEndpoints
{
    public static void MapTtaEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/ttas", async () =>
{
    try
    {
        var cDb = new ClaseDB<TtaModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM BookingTransports ORDER BY Id;");

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
app.MapGet("/api/ttas/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TtaModel>();
        var data = await cDb.GetAsync($"SELECT * FROM BookingTransports WHERE Id = {id};");

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
app.MapPost("/api/ttas", async (TtaModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TtaModel>();
        data.Id = await cDb.ExecuteInsertAsync(data, "INSERT INTO BookingTransports (Container,Sello,Tara,Packing,FechaCarga,Conductor,Patente,Telefono,Rut,BookingId) VALUES (@Container,@Sello,@Tara,@Packing,@FechaCarga,@Conductor,@Patente,@Telefono,@Rut,@BookingId);");

        return Results.Created($"/ttas", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/ttas", async (TtaModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TtaModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE BookingTransports SET Container=@Container,Sello=@Sello,Tara=@Tara,Packing=@Packing,FechaCarga=@FechaCarga,Conductor=@Conductor,Patente=@Patente,Telefono=@Telefono,Rut=@Rut,BookingId=@BookingId WHERE Id=@Id;");

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
app.MapDelete("/api/ttas/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TtaModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM BookingTransports WHERE Id={id};");

        return new
        {
            Message = "Tts deleted"
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
