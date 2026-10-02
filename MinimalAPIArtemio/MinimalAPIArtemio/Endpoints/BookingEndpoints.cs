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

public static class BookingEndpoints
{
    public static void MapBookingEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/booking", async () =>
{
    try
    {
        var cDb = new ClaseDB<BookingModel>();
        var data = await cDb.GetAllAsync($"SELECT * FROM Booking ORDER BY BookingId DESC;");

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
app.MapGet("/api/bookinggrid/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingGrid>();
        var data = await cDb.GetAllAsync(@$"SELECT
	                                        b.`BookingId`,
	                                        IF(WEEK(IFNULL(b.`LastETD`,DATE('1900-01-01')),3) = 0,1,WEEK(IFNULL(b.`LastETD`,DATE('1900-01-01')),3)) AS ETDWeek,
	                                        IFNULL(b.`LastETD`,DATE('1900-01-01')) AS ETD,
                                            IFNULL(b.`OriginalETA`,DATE('1900-01-01')) AS ETA,
	                                        b.`NroBooking`,
	                                        b.`NroRequest`,
	                                        b.`PoRef` AS PO,
	                                        IFNULL(ve.`Vessel`,'') AS Vessel,
	                                        b.`Voyage`,
	                                        b.`BookingStatus`,
	                                        IFNULL(tc.`TransportCompany`,'') AS ShippingCompany,
	                                        IFNULL(pol.`Port`,'') AS POL,
	                                        IFNULL(pod.`Port`,'') AS POD,
	                                        IFNULL(sh.`Shipper`,'') AS Shipper,
	                                        IFNULL(co.`Commodity`,'') AS Commodity,
	                                        IFNULL(b.`QtyContainers`,0) AS QtyContainers,
	                                        IFNULL((SELECT NULLIF(GROUP_CONCAT(DISTINCT NULLIF(TRIM(bd.BL), '')), '') FROM BookingDetails bd WHERE bd.BookingId = b.BookingId),'-') AS BL,
	                                        IFNULL((SELECT NULLIF(GROUP_CONCAT(DISTINCT NULLIF(TRIM(bd.Container), '')), '') FROM BookingDetails bd WHERE bd.BookingId = b.BookingId),'-') AS Container
                                        FROM Booking b
                                        LEFT JOIN Vessels ve ON ve.`VesselId` = b.`VesselId`
                                        LEFT JOIN TransportCompanies tc ON tc.`TransportCompanyId` = b.`TransportCompanyId`
                                        LEFT JOIN Ports pol ON pol.`PortId` = b.`POLId`
                                        LEFT JOIN Ports pod ON pod.`PortId` = b.`LastPODId`
                                        LEFT JOIN Shippers sh ON sh.`ShipperId` = b.`ShipperId`
                                        LEFT JOIN Commodities co ON co.`CommodityId` = b.`CommodityId`
                                        WHERE b.SeasonId = {id}
                                        ORDER BY b.`LastETD` DESC;");

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
app.MapGet("/api/bookingcontractgrid/{POLId}/{PODId}/{TransportCompanyId}", async (int POLId,int PODId, int TransportCompanyId) =>
{
    try
    {
        var cDb = new ClaseDB<BookingContractGridModel>();
        var data = await cDb.GetAllAsync(@$"SELECT
                                            c.NroContract,
	                                        IFNULL(sh.`Shipper`,'') AS Shipper,
	                                        IFNULL(i.`Intermediary`,'') AS Intermediary,
	                                        cc.`Public`,
	                                        cc.`Net`,
	                                        cc.FeeShipper,
	                                        cc.FeeIntermediary,
	                                        cc.Comm,
	                                        cc.ContractId 
                                        FROM ContractsConditions cc 
                                        LEFT JOIN Shippers sh ON sh.`ShipperId` = cc.`ShipperId` 
                                        LEFT JOIN Intermediaries i ON i.`IntermediaryId` = cc.`IntermediaryId` 
                                        LEFT JOIN Contracts c ON c.`ContractId` = cc.`ContractId` 
                                        WHERE c.`POLId` = {POLId} AND c.`PODId` = {PODId} AND c.`TransportCompanyId` = {TransportCompanyId};");

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

app.MapGet("/api/bookingtt/{ServiceId}/{POLId}/{ETD}/{PODId}", async (int ServiceId, int POLId, DateTime ETD, int PODId) =>
{
    try
    {
        var cDb = new ClaseDB<int>();
        var data = await cDb.GetAllAsync(@$"SELECT
	                                            t.TT
                                            FROM `TtsDetails` td
                                            LEFT JOIN TTs t ON t.`Id` = td.`Id`
                                            WHERE t.`ServiceId` = {ServiceId} AND t.`POLId` = {POLId} AND '{ETD.ToShortDateString()}' BETWEEN t.`_From` AND t.`_To` AND td.`PODId` = {PODId};");

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
app.MapGet("/api/booking/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingModel>();
        var data = await cDb.GetAsync($"SELECT * FROM Booking WHERE BookingId = {id};");

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
app.MapPost("/api/booking", async (BookingModel data) =>
{
    try
    {
        var cDb = new ClaseDB<BookingModel>();
        data.HsCode ??= "";
        data.BookingId = await cDb.ExecuteInsertAsync(data, @"INSERT INTO Booking (BookingDate,SendEmailDate,UploadDate,BookingStatus,ContractCo,TransportCompanyId,AgentId,AgentEmbarqueId,
                                            IntermediaryId,TruckArtemio,TransportLand,DUS,Weight,BlIssuedId,ContractTypeId,PoRef,ShipperId,ServiceId,VesselId,Voyage,
                                            NroRequest,NroBooking,AdelantoFolio,WarehouseId,StackingFrom,StackingUntil,LAR,LARDoc,ExtraLAR,CutOf,LastETD,LastETA,POLId,Transport,LastPODId,DeliveryPlace,ZipCode,
                                            Address,QtyContainers,QtyContainersReal,ContainerTypeId,ColdTreatment,CommodityId,Temperature,Unit,CBM,PorcCBM,PaymentConditionId,StatusBL,
                                            Roleo,RoleoBy,IncotermId,FreeDaysPOL,FreeDaysPOD,Net,Public,Rebate,InvoiceLAR,AmountLAR,InvoiceLARDoc,AmountLARDoc,ReleaseBL,Status,Comments,SeasonId,OriginalETD,OriginalETA,OriginalPODId,Humidity,TransportId,AgentAduanaId,NroContract,FeeShipper,FeeIntermediary,ArtemioCom,ContractId,TT,FeeArtemio,StackingEmail,HsCode) VALUES 
                                            (@BookingDate,@SendEmailDate,@UploadDate,@BookingStatus,@ContractCo,@TransportCompanyId,@AgentId,@AgentEmbarqueId,
                                            @IntermediaryId,@TruckArtemio,@TransportLand,@DUS,@Weight,@BlIssuedId,@ContractTypeId,@PoRef,@ShipperId,@ServiceId,@VesselId,@Voyage,
                                            @NroRequest,@NroBooking,@AdelantoFolio,@WarehouseId,@StackingFrom,@StackingUntil,@LAR,@LARDoc,@ExtraLAR,@CutOf,@LastETD,@LastETA,@POLId,@Transport,@LastPODId,@DeliveryPlace,@ZipCode,
                                            @Address,@QtyContainers,@QtyContainersReal,@ContainerTypeId,@ColdTreatment,@CommodityId,@Temperature,@Unit,@CBM,@PorcCBM,@PaymentConditionId,@StatusBL,
                                            @Roleo,@RoleoBy,@IncotermId,@FreeDaysPOL,@FreeDaysPOD,@Net,@Public,@Rebate,@InvoiceLAR,@AmountLAR,@InvoiceLARDoc,@AmountLARDoc,@ReleaseBL,@Status,@Comments,@SeasonId,@OriginalETD,@OriginalETA,@OriginalPODId,@Humidity,@TransportId,@AgentAduanaId,@NroContract,@FeeShipper,@FeeIntermediary,@ArtemioCom,@ContractId,@TT,@FeeArtemio,0,@HsCode);");

        await cDb.ExcecuteQueryAsync(data, "UPDATE Booking SET StackingFrom=@StackingFrom,StackingUntil=@StackingUntil WHERE VesselId = @VesselId AND Voyage = @Voyage AND SeasonId = @SeasonId;");

        return Results.Created($"/booking", data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = string.IsNullOrWhiteSpace(ex.Message) ? "Error" : ex.Message
        });
    }
});
app.MapPut("/api/booking", async (BookingModel data) =>
{
    try
    {
        var cDb = new ClaseDB<BookingModel>();
        data.HsCode ??= "";
        await cDb.ExcecuteQueryAsync(data, @"UPDATE Booking SET BookingDate=@BookingDate,SendEmailDate=@SendEmailDate,UploadDate=@UploadDate,BookingStatus=@BookingStatus,ContractCo=@ContractCo,
                                            TransportCompanyId=@TransportCompanyId,AgentId=@AgentId,AgentEmbarqueId=@AgentEmbarqueId,IntermediaryId=@IntermediaryId,TruckArtemio=@TruckArtemio,TransportLand=@TransportLand,
                                            DUS=@DUS,Weight=@Weight,BlIssuedId=@BlIssuedId,ContractTypeId=@ContractTypeId,PoRef=@PoRef,ShipperId=@ShipperId,ServiceId=@ServiceId,VesselId=@VesselId,
                                            Voyage=@Voyage,NroRequest=@NroRequest,NroBooking=@NroBooking,AdelantoFolio=@AdelantoFolio,WarehouseId=@WarehouseId,StackingFrom=@StackingFrom,StackingUntil=@StackingUntil,LAR=@LAR,
                                            LARDoc=@LARDoc,ExtraLAR=@ExtraLAR,CutOf=@CutOf,LastETD=@LastETD,LastETA=@LastETA,POLId=@POLId,Transport=@Transport,LastPODId=@LastPODId,DeliveryPlace=@DeliveryPlace,
                                            ZipCode=@ZipCode,Address=@Address,QtyContainers=@QtyContainers,QtyContainersReal=@QtyContainersReal,ContainerTypeId=@ContainerTypeId,ColdTreatment=@ColdTreatment,CommodityId=@CommodityId,Temperature=@Temperature,
                                            Unit=@Unit,CBM=@CBM,PorcCBM=@PorcCBM,PaymentConditionId=@PaymentConditionId,StatusBL=@StatusBL,Roleo=@Roleo,RoleoBy=@RoleoBy,
                                            IncotermId=@IncotermId,FreeDaysPOL=@FreeDaysPOL,FreeDaysPOD=@FreeDaysPOD,Net=@Net,Public=@Public,Rebate=@Rebate,InvoiceLAR=@InvoiceLAR,ReleaseBL=@ReleaseBL,
                                            Status=@Status,Comments=@Comments,OriginalETA=@OriginalETA,Humidity=@Humidity,TransportId=@TransportId,AgentAduanaId=@AgentAduanaId,NroContract=@NroContract,FeeShipper=@FeeShipper,FeeIntermediary=@FeeIntermediary,ArtemioCom=@ArtemioCom,ContractId=@ContractId,TT=@TT,FeeArtemio=@FeeArtemio,HsCode=@HsCode WHERE BookingId=@BookingId;");


        await cDb.ExcecuteQueryAsync(data, "UPDATE Booking SET StackingFrom=@StackingFrom,StackingUntil=@StackingUntil WHERE VesselId = @VesselId AND Voyage = @Voyage AND SeasonId = @SeasonId;");
        return Results.Ok(data);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = string.IsNullOrWhiteSpace(ex.Message) ? "Error" : ex.Message
        });
    }
});
app.MapDelete("/api/booking/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingModel>();
        await cDb.ExcecuteQueryAsync($"DELETE FROM Booking WHERE BookingId={id};");

        return new
        {
            Message = "Booking deleted"
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

app.MapPost("/api/copybooking/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingModel>();
        var newId = await cDb.ExecuteInsertSqlAsync(@"INSERT INTO Booking (
                                            BookingDate,SendEmailDate,UploadDate,BookingStatus,ContractCo,TransportCompanyId,AgentId,
                                            AgentEmbarqueId,AgentAduanaId,IntermediaryId,TruckArtemio,TransportLand,DUS,Weight,BlIssuedId,
                                            ContractTypeId,PoRef,ShipperId,ServiceId,VesselId,Voyage,NroContract,NroRequest,NroBooking,
                                            AdelantoFolio,WarehouseId,StackingFrom,StackingUntil,LAR,LARDoc,ExtraLAR,CutOf,LastETD,
                                            OriginalETD,OriginalETA,LastETA,TT,POLId,Transport,LastPODId,OriginalPODId,DeliveryPlace,
                                            ZipCode,Address,QtyContainers,QtyContainersReal,ContainerTypeId,ColdTreatment,CommodityId,
                                            Temperature,Unit,CBM,PorcCBM,Humidity,PaymentConditionId,StatusBL,Roleo,RoleoBy,IncotermId,
                                            FreeDaysPOL,FreeDaysPOD,Net,Public,Rebate,InvoiceLAR,AmountLAR,InvoiceLARDoc,AmountLARDoc,
                                            ReleaseBL,Status,Comments,SeasonId,ConsigneeId,NotifyId,TransportId,FeeShipper,FeeIntermediary,
                                            ArtemioCom,ContractId,FeeArtemio,HsCode,StackingEmail)
                                            SELECT BookingDate,SendEmailDate,UploadDate,BookingStatus,ContractCo,TransportCompanyId,AgentId,
                                            AgentEmbarqueId,AgentAduanaId,IntermediaryId,TruckArtemio,TransportLand,DUS,Weight,BlIssuedId,
                                            ContractTypeId,PoRef,ShipperId,ServiceId,VesselId,Voyage,NroContract,NroRequest,NroBooking,
                                            AdelantoFolio,WarehouseId,StackingFrom,StackingUntil,LAR,LARDoc,ExtraLAR,CutOf,LastETD,
                                            OriginalETD,OriginalETA,LastETA,TT,POLId,Transport,LastPODId,OriginalPODId,DeliveryPlace,
                                            ZipCode,Address,QtyContainers,QtyContainersReal,ContainerTypeId,ColdTreatment,CommodityId,
                                            Temperature,Unit,CBM,PorcCBM,Humidity,0,StatusBL,Roleo,RoleoBy,IncotermId,
                                            FreeDaysPOL,FreeDaysPOD,Net,Public,Rebate,InvoiceLAR,AmountLAR,InvoiceLARDoc,AmountLARDoc,
                                            ReleaseBL,Status,Comments,SeasonId,ConsigneeId,NotifyId,TransportId,FeeShipper,FeeIntermediary,
                                            ArtemioCom,ContractId,FeeArtemio,IFNULL(HsCode,''),0
                                            FROM Booking WHERE BookingId = @id;", new { id });

        if (newId <= 0)
        {
            return Results.BadRequest(new { Message = "Error duplicating booking" });
        }

        await cDb.ExcecuteQueryAsync(@"INSERT INTO BookingDetails
                                            (BL,Container,ConsigneeId,NotifyId,Cases,StatusContainerId,Comment,BookingId,Comm,Rebate,Fee,RebateExp,SendFreeDays,SendFreeDaysComment)
                                            SELECT BL,Container,ConsigneeId,NotifyId,Cases,StatusContainerId,Comment,@newId,Comm,Rebate,Fee,RebateExp,SendFreeDays,SendFreeDaysComment
                                            FROM BookingDetails WHERE BookingId = @id;", new { id, newId });

        await cDb.ExcecuteQueryAsync(@"INSERT INTO BookingETA (Nro,ETA,BookingId)
                                            SELECT Nro,ETA,@newId FROM BookingETA WHERE BookingId = @id;", new { id, newId });

        await cDb.ExcecuteQueryAsync(@"INSERT INTO BookingPOD (PODId,Comment,BookingId)
                                            SELECT PODId,Comment,@newId FROM BookingPOD WHERE BookingId = @id;", new { id, newId });

        var data = await cDb.GetAsync("SELECT * FROM Booking WHERE BookingId = @newId;", new { newId });

        return Results.Created($"/booking", data);
    }
    catch (Exception)
    {
        return Results.BadRequest(new
        {
            Message = "Error duplicating booking"
        });
    }
});
app.MapGet("/api/bookinghscode", async () =>
{
    var db = new ClaseDB<BookingHsCodeGrid>();
    var data = await db.GetAllAsync(@"SELECT
                                            sh.ShipperId,
                                            co.CommodityId,
                                            sh.Shipper,
                                            co.Commodity,
                                            IFNULL(MAX(b.HsCode),'') AS HsCode
                                        FROM Booking b
                                        LEFT JOIN Shippers sh ON sh.ShipperId = b.ShipperId
                                        LEFT JOIN Commodities co ON co.CommodityId = b.CommodityId
                                        GROUP BY sh.ShipperId, co.CommodityId, sh.Shipper, co.Commodity;");
    return Results.Ok(data);
});
app.MapPut("/api/bookinghscode", async (List<BookingHsCodeGrid> body) =>
{
    var db = new ClaseDB<BookingHsCodeGrid>();
    foreach (var item in body)
    {
        await db.ExcecuteQueryAsync("UPDATE Booking SET HsCode=@HsCode WHERE ShipperId=@ShipperId AND CommodityId=@CommodityId;", item);
    }
    return Results.Ok(true);
});
    }
}
