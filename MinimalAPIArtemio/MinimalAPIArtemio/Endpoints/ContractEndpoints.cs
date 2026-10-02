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

public static class ContractEndpoints
{
    public static void MapContractEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/contracts", async () =>
{
    try
    {
        var cDb = new ClaseDB<ContractModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM Contracts ORDER BY ContractId DESC;");

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
app.MapGet("/api/contractsgrid", async () =>
{
    try
    {
        var cDb = new ClaseDB<ContractGrid>();
        var data = await cDb.GetAllAsync(@$"SELECT
	                                        c.`ContractId`,
	                                        tc.`TransportCompany`,
	                                        c.`NroContract`,
	                                        c.`From`,
	                                        c.`Until`,
	                                        co.`Commodity`,
	                                        pol.`Port` AS POL,
	                                        pod.`Port` AS POD,
	                                        c.`TT`,
	                                        c.`Public`,
	                                        c.`Net`,
	                                        c.`VolumeDiscount` as VD,
	                                        ifnull((select sum(cd.Amount) from `ContractsDetails` cd where cd.ContractId = c.`ContractId` and cd.Include = 'Not Include'),0) as Sum,
                                            c.RateType 
                                        FROM Contracts c
                                        LEFT JOIN TransportCompanies tc ON tc.`TransportCompanyId` = c.`TransportCompanyId`
                                        LEFT JOIN Commodities co ON co.`CommodityId` = c.`CommodityId`
                                        LEFT JOIN Ports pol ON pol.`PortId` = c.`POLId`
                                        LEFT JOIN Ports pod ON pod.`PortId` = c.`PODId` 
                                        ORDER BY c.`ContractId` DESC;");

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
app.MapGet("/api/contracts/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ContractModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Contracts WHERE ContractId = {id};");

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
app.MapPost("/api/contracts", async (ContractModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ContractModel>();
        data.ContractId = await cDb.ExecuteInsertAsync(data, @"INSERT INTO Contracts (TransportCompanyId ,NroContract,`From`,Until, CommodityId, POLId, POT1, POT2,POT3, PODId, TT, FreeDaysOrigin, FreeDaysDestination, Public, VolumeDiscount, Net, FreeArtemio, ServiceId, Comment, SeasonId, ShipperId, Status, RateType) VALUES 
                                                                  (@TransportCompanyId,@NroContract,@From,@Until,@CommodityId,@POLId,@POT1,@POT2,@POT3,@PODId,@TT,@FreeDaysOrigin,@FreeDaysDestination,@Public,@VolumeDiscount,@Net,@FreeArtemio,@ServiceId,@Comment,@SeasonId,@ShipperId,@Status,@RateType);");

        return Results.Created($"/contracts", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPost("/api/copycontracts/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingModel>();
        var newId = await cDb.ExecuteInsertSqlAsync(@"INSERT INTO Contracts (TransportCompanyId,NroContract,`From`,Until,CommodityId,POLId,POT1,POT2,POT3,PODId,TT,FreeDaysOrigin,FreeDaysDestination,Public,VolumeDiscount,Net,FreeArtemio,ServiceId,Comment,SeasonId, ShipperId, Status, RateType)
                                                        SELECT TransportCompanyId,NroContract,`From`,Until,CommodityId,POLId,POT1,POT2,POT3,PODId,TT,FreeDaysOrigin,FreeDaysDestination,Public,VolumeDiscount,Net,FreeArtemio,ServiceId,Comment,SeasonId, ShipperId, Status, RateType FROM Contracts WHERE ContractId = @id;", new { id });

        await cDb.ExcecuteQueryAsync($@"INSERT INTO ContractsDetails (CostItemId,Amount,Include,ContractId)
                                                               SELECT CostItemId,Amount,Include,{newId} FROM ContractsDetails WHERE ContractId = {id};");

        var data = await cDb.GetAsync($"SELECT * FROM Contracts WHERE ContractId = {newId}");

        return Results.Created($"/contracts", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
app.MapPut("/api/contracts", async (ContractModel data) =>
{
    try
    {
        var cDb = new ClaseDB<ContractModel>();
        await cDb.ExcecuteQueryAsync(data, @"UPDATE Contracts SET TransportCompanyId=@TransportCompanyId ,NroContract=@NroContract, `From`=@From, Until=@Until, CommodityId=@CommodityId, POLId=@POLId, POT1=@POT1, POT2=@POT2, POT3=@POT3, PODId=@PODId, TT=@TT, FreeDaysOrigin=@FreeDaysOrigin,FreeDaysDestination=@FreeDaysDestination, Public=@Public, VolumeDiscount=@VolumeDiscount, Net=@Net, FreeArtemio=@FreeArtemio, ServiceId=@ServiceId,Comment=@Comment, ShipperId=@ShipperId,Status=@Status,RateType=@RateType WHERE ContractId=@ContractId;");

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
app.MapDelete("/api/contracts/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<ContractModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Contracts WHERE ContractId={id};");

        return new
        {
            Message = "Contract deleted"
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
