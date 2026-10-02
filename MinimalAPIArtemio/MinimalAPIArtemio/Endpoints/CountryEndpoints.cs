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

public static class CountryEndpoints
{
    public static void MapCountryEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/countries", async () =>
{
    try
    {
        var cDb = new ClaseDB<CountryModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Countries ORDER BY Country;");

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
app.MapGet("/api/countries/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<CountryModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Countries WHERE CountryId = {id};");

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
app.MapPost("/api/countries", async (CountryModel data) =>
{
    try
    {
        var cDb = new ClaseDB<CountryModel>();
        data.CountryId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Countries (Country, FreeDaysComment) VALUES (@Country, @FreeDaysComment);");

        return Results.Created($"/countries", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/countries", async (CountryModel data) =>
{
    try
    {
        var cDb = new ClaseDB<CountryModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Countries SET Country=@Country, FreeDaysComment=@FreeDaysComment WHERE CountryId=@CountryId;");

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
app.MapDelete("/api/countries/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<CountryModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Countries WHERE CountryId={id};");

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
