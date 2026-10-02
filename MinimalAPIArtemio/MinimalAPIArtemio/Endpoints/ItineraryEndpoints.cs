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

public static class ItineraryEndpoints
{
    public static void MapItineraryEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/itinerariesgrid", async () =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryGridModel>();
        var data = await cDb.GetAllAsync(@$"
            SELECT
	            i.ItineraryId,
	            i.`Week`,
	            i.`ETD`,
	            IFNULL(tc.`TransportCompany`,'') AS TransportCompany,
	            IFNULL(s.`Service`,'') AS Service,
	            IFNULL(v.`Vessel`,'') AS Vessel,
	            IFNULL(pol.`Port`,'') AS POL
            FROM Itineraries i
            LEFT JOIN Vessels v ON v.`VesselId` = i.`VesselId`
            LEFT JOIN Services s ON s.`ServiceId` = i.`ServiceId`
            LEFT JOIN Ports pol ON pol.`PortId` = i.`POLId`
            LEFT JOIN TransportCompanies tc ON tc.`TransportCompanyId` = i.`OperatorId` 
            ORDER BY i.ETD DESC;");

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
app.MapGet("/api/itineraries", async () =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryModel>();
        var data = await cDb.GetAllAsync("SELECT * FROM Itineraries ORDER BY ETD DESC;");

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
app.MapGet("/api/itineraries/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Itineraries WHERE ItineraryId = {id};");

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
app.MapPost("/api/copyitineraries/{id}/{add}", async (int id, int add) =>
{
    try
    {
        var cDb = new ClaseDB<BookingModel>();
        var newId = await cDb.ExecuteInsertSqlAsync(@"INSERT INTO Itineraries (Week,ETD,POLId,VesselId,ServiceId,OperatorId,SeasonId,Voyage)
            SELECT Week(adddate(ETD,@add),3) as Week,adddate(ETD,@add) as ETD,POLId,VesselId,ServiceId,OperatorId,SeasonId,Voyage
            FROM Itineraries WHERE ItineraryId = @id;", new { id, add });
        var data = await cDb.GetAsync("SELECT * FROM Itineraries WHERE ItineraryId = @newId;", new { newId });

        var cDbDtl = new ClaseDB<ItineraryDetailModel>();
        await cDbDtl.ExcecuteQueryAsync($"INSERT INTO ItinerariesDetails (PODId,ETA,TT,ItineraryId) SELECT PODId,adddate(ETA,{add}) as ETA,(TT + {add}) as TT,{newId} as ItineraryId FROM ItinerariesDetails WHERE ItineraryId = {id};");

        var cDbETD = new ClaseDB<ItineraryETDModel>();
        await cDbETD.ExcecuteQueryAsync($"INSERT INTO ItinerariesETD (POLId,ETD,IsMain,ItineraryId) SELECT POLId,adddate(ETD,{add}),IsMain,{newId} as ItineraryId FROM ItinerariesETD WHERE ItineraryId = {id};");

        var cDbTC = new ClaseDB<ItineraryTransportCompanyModel>();
        await cDbTC.ExcecuteQueryAsync($"INSERT INTO ItinerariesTransportCompanies (TransportCompanyId,ItineraryId) SELECT TransportCompanyId,{newId} as ItineraryId FROM ItinerariesTransportCompanies WHERE ItineraryId = {id};");

        return Results.Created($"/Itineraries", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPost("/api/itineraries", async (ItineraryModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryModel>();
        data.ItineraryId = await cDb.ExecuteInsertAsync(data, "INSERT INTO Itineraries (Week,ETD,POLId,VesselId,ServiceId,OperatorId,SeasonId,Voyage,Stacking,LateArrival,ExtraLateArrival,CutOff) VALUES (@Week,@ETD,@POLId,@VesselId,@ServiceId,@OperatorId,@SeasonId,@Voyage,@Stacking,@LateArrival,@ExtraLateArrival,@CutOff);");

        return Results.Created($"/itineraries", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/itineraries", async (ItineraryModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryModel>();
        await cDb.ExcecuteQueryAsync(data, "UPDATE Itineraries SET Week=@Week,ETD=@ETD,POLId=@POLId,VesselId=@VesselId,ServiceId=@ServiceId,OperatorId=@OperatorId,Voyage=@Voyage,Stacking=@Stacking,LateArrival=@LateArrival,ExtraLateArrival=@ExtraLateArrival,CutOff=@CutOff WHERE ItineraryId=@ItineraryId;");

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
app.MapDelete("/api/itineraries/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<int>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Itineraries WHERE ItineraryId={id};");

        return new
        {
            Message = "Intermediry deleted"
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
