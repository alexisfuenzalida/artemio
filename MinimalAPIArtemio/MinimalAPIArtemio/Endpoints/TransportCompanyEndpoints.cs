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

public static class TransportCompanyEndpoints
{
    public static void MapTransportCompanyEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/transportcompanies", async () =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM TransportCompanies ORDER BY TransportCompany;");

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
app.MapGet("/api/transportcompanies/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyModel>();
        var data = await cDb.GetAsync($"SELECT * FROM TransportCompanies WHERE TransportCompanyId = {id};");

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
app.MapPost("/api/transportcompanies", async (TransportCompanyModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyModel>();
        data.TransportCompanyId = await cDb.ExecuteInsertAsync(data, "INSERT INTO TransportCompanies (TransportCompany) VALUES (@TransportCompany);");

        return Results.Created($"/transportcompanies", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/transportcompanies", async (TransportCompanyModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE TransportCompanies SET TransportCompany=@TransportCompany WHERE TransportCompanyId=@TransportCompanyId;");

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
app.MapDelete("/api/transportcompanies/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM TransportCompanies WHERE TransportCompanyId={id};");

        return new
        {
            Message = "Transport Company deleted"
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

app.MapGet("/api/tcsassigned/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ServiceModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM Services WHERE ServiceId IN (SELECT ServiceId FROM TransportCompaniesServices WHERE TransportCompanyId = {id});");

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
app.MapGet("/api/tcsavailable/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ServiceModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM Services WHERE ServiceId NOT IN (SELECT ServiceId FROM TransportCompaniesServices WHERE TransportCompanyId = {id});");

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
app.MapGet("/api/tcs/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ServiceModel>();
        var data = await cDb.GetAllAsync($"SELECT ServiceId FROM TransportCompaniesServices WHERE TransportCompanyId = {id};");

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
        Console.WriteLine(ex.Message);
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPost("/api/tcs", async (TransportCompanyServiceModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyServiceModel>();
        await cDb.ExcecuteQueryAsync(data, "INSERT INTO TransportCompaniesServices (TransportCompanyId,ServiceId) VALUES (@TransportCompanyId,@ServiceId);");

        return Results.Created($"/tcs", data);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/tcs/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyServiceModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM TransportCompaniesServices WHERE TransportCompanyId={id};");

        return new
        {
            Message = "Transport Company deleted"
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
//TransportCompanyServiceModel


app.MapGet("/api/tci/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyItemModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM TransportCompaniesItems WHERE TransportCompanyId = {id};");

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
        Console.WriteLine(ex.Message);
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPost("/api/tci", async (TransportCompanyItemModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyItemModel>();
        await cDb.ExcecuteQueryAsync(data, "INSERT INTO TransportCompaniesItems (Service,Item,Rate,Type,TransportCompanyId) VALUES (@Service,@Item,@Rate,@Type,@TransportCompanyId);");

        return Results.Created($"/tci", data);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/tci/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyItemModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM TransportCompaniesItems WHERE TransportCompanyId={id};");

        return new
        {
            Message = "Transport Company deleted"
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
//TransportCompanyItemModel


app.MapGet("/api/tcpolassigned/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<PortModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM Ports WHERE PortId IN (SELECT PortId FROM TransportCompaniesPOL WHERE TransportCompanyId = {id}) ORDER BY Port;");

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
app.MapGet("/api/tcpolavailable/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<PortModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM Ports WHERE PortId NOT IN (SELECT PortId FROM TransportCompaniesPOL WHERE TransportCompanyId = {id}) ORDER BY Port;");

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
app.MapPost("/api/tcpol", async (TransportCompanyPolModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyPolModel>();
        await cDb.ExcecuteQueryAsync(data, "INSERT INTO TransportCompaniesPOL (TransportCompanyId,PortId) VALUES (@TransportCompanyId,@PortId);");

        return Results.Created($"/tcpol", data);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/tcpol/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyPolModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM TransportCompaniesPOL WHERE TransportCompanyId={id};");

        return new
        {
            Message = "Transport Company deleted"
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
//TransportCompanyPolModel


app.MapGet("/api/tcpodassigned/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<PortModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM Ports WHERE PortId IN (SELECT PortId FROM TransportCompaniesPOD WHERE TransportCompanyId = {id}) ORDER BY Port;");

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
app.MapGet("/api/tcpodavailable/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<PortModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM Ports WHERE PortId NOT IN (SELECT PortId FROM TransportCompaniesPOD WHERE TransportCompanyId = {id}) ORDER BY Port;");

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
app.MapPost("/api/tcpod", async (TransportCompanyPodModel data) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyPodModel>();
        await cDb.ExcecuteQueryAsync(data, "INSERT INTO TransportCompaniesPOD (TransportCompanyId,PortId) VALUES (@TransportCompanyId,@PortId);");

        return Results.Created($"/tcpod", data);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapDelete("/api/tcpod/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<TransportCompanyPodModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM TransportCompaniesPOD WHERE TransportCompanyId={id};");

        return new
        {
            Message = "Transport Company deleted"
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
