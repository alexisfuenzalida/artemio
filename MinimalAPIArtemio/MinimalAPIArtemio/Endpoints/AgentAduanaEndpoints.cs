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

public static class AgentAduanaEndpoints
{
    public static void MapAgentAduanaEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/agentsaduanas", async () =>
{
    try
    {
        var cDb = new ClaseDB<AgentModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM AgentsAduanas ORDER BY Agent;");

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
app.MapGet("/api/agentsaduanas/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<AgentModel>();
        var data = await cDb.GetAsync($"SELECT * FROM AgentsAduanas WHERE AgentId = {id};");

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
app.MapPost("/api/agentsaduanas", async (AgentModel data) =>
{
    try
    {
        var cDb = new ClaseDB<AgentModel>();
        data.AgentId = await cDb.ExecuteInsertAsync(data, "INSERT INTO AgentsAduanas (Agent) VALUES (@Agent);");

        return Results.Created($"/Agentsaduanas", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/agentsaduanas", async (AgentModel data) =>
{
    try
    {
        var cDb = new ClaseDB<AgentModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE AgentsAduanas SET Agent=@Agent WHERE AgentId=@AgentId;");

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
app.MapDelete("/api/agentsaduanas/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<AgentModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM AgentsAduanas WHERE AgentId={id};");

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
