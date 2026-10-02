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

public static class ShipperEndpoints
{
    public static void MapShipperEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/shippers", async () =>
{
    try
    {
        var cDb = new ClaseDB<ShipperModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Shippers ORDER BY Shipper;");

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
app.MapGet("/api/shippers/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ShipperModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Shippers WHERE ShipperId = {id};");

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
app.MapPost("/api/shippers", async (ShipperModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ShipperModel>();
        data.ShipperId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Shippers (Code,Shipper,Rut,Address,CityId,Comment,Enabled) VALUES (@Code,@Shipper,@Rut,@Address,@CityId,@Comment,@Enabled);");

        return Results.Created($"/shippers", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/shippers", async (ShipperModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ShipperModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Shippers SET Code=@Code,Shipper=@Shipper,Rut=@Rut,Address=@Address,CityId=@CityId,Comment=@Comment,Enabled=@Enabled WHERE ShipperId=@ShipperId;");

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
app.MapDelete("/api/shippers/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ShipperModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Shippers WHERE ShipperId={id};");

        return new
        {
            Message = "Shipper deleted"
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
app.MapGet("/api/shipperscontacts/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ShipperContactModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM ShippersContacts WHERE ShipperId = {id} ORDER BY Contact;");

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
app.MapPost("/api/shipperscontacts", async (ShipperContactModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ShipperContactModel>();
        await cDb.ExcecuteQueryAsync(data,"INSERT INTO ShippersContacts (Contact,Phone,Email,ShipperId) VALUES (@Contact,@Phone,@Email,@ShipperId);");

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
app.MapDelete("/api/shipperscontacts/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ShipperContactModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM ShippersContacts WHERE ShipperId={id};");

        return new
        {
            Message = "Shipper Contacts deleted"
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
