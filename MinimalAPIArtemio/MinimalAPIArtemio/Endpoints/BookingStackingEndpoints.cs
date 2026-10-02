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

public static class BookingStackingEndpoints
{
    public static void MapBookingStackingEndpoints(this IEndpointRouteBuilder app)
    {
app.MapGet("/api/bookingstacking", async () =>
{
    try
    {
        var cDb = new ClaseDB<BookingStackingGrid>();
        var data = await cDb.GetAllAsync(@$"SELECT
                                                b.LastETD,
                                                tc.`TransportCompanyId`,
	                                            tc.`TransportCompany`,
	                                            ve.`VesselId`, 
	                                            ve.`Vessel`,
                                                b.Voyage,
                                                b.POLId,
                                                IFNULL(pol.`Port`,'') AS POL,
                                                b.PoRef
                                            FROM Booking b
                                            INNER JOIN `TransportCompanies` tc ON tc.`TransportCompanyId` = b.`TransportCompanyId`
                                            INNER JOIN `Vessels` ve ON ve.`VesselId` = b.`VesselId`
                                            LEFT JOIN Ports pol ON pol.`PortId` = b.`POLId`
                                            WHERE b.`StackingEmail` = 0
                                            GROUP BY b.LastETD,b.TransportCompanyId,b.VesselId,b.Voyage,b.POLId
                                            ORDER BY b.LastETD DESC,ve.Vessel,tc.TransportCompany;");

        return Results.Ok(data);
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
app.MapPut("/api/bookingstacking", async (List<BookingStackingGrid> body) =>
{
    try
    {
        var cDb = new ClaseDB<BookingStackingGrid>();

        foreach (var item in body)
        {
            await cDb.ExcecuteQueryAsync("UPDATE Booking b SET b.StackingEmail = 1 WHERE b.`TransportCompanyId` = @TransportCompanyId AND b.`VesselId` = @VesselId AND b.Voyage = @Voyage AND b.POLId = @POLId;", item);
        }

        return Results.Ok(true);
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
    }
}
