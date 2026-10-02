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

public static class CityEndpoints
{
    public static void MapCityEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/cities", async () =>
{
    try
    {
        var cDb = new ClaseDB<CityModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Cities ORDER BY City;");

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
app.MapGet("/api/cities/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<CityModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Cities WHERE CityId = {id};");

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
app.MapPost("/api/cities", async (CityModel data) =>
{
    try
    {
        var cDb = new ClaseDB<CityModel>();
        data.CityId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Cities (City,CountryId) VALUES (@City,@CountryId);");

        return Results.Created($"/cities", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/cities", async (CityModel data) =>
{
    try
    {
        var cDb = new ClaseDB<CityModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Cities SET City=@City,CountryId=@CountryId WHERE CityId=@CityId;");

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
app.MapDelete("/api/cities/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<CityModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Cities WHERE CityId={id};");

        return new
        {
            Message = "City deleted"
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
