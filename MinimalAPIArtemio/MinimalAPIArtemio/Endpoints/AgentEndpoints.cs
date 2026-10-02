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

public static class AgentEndpoints
{
    public static void MapAgentEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/agents", async () =>
{
    try
    {
        var cDb = new ClaseDB<AgentModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Agents ORDER BY Agent;");

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
app.MapGet("/api/agents/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<AgentModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Agents WHERE AgentId = {id};");

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
app.MapPost("/api/agents", async (AgentModel data) =>
{
    try
    {
        var cDb = new ClaseDB<AgentModel>();
        data.AgentId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Agents (Agent) VALUES (@Agent);");

        return Results.Created($"/Involved", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/agents", async (AgentModel data) =>
{
    try
    {
        var cDb = new ClaseDB<AgentModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Agents SET Agent=@Agent WHERE AgentId=@AgentId;");

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
app.MapDelete("/api/agents/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<AgentModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Agents WHERE AgentId={id};");

        return new
        {
            Message = "Agent deleted"
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
