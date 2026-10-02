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

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this IEndpointRouteBuilder app)
    {

app.MapGet("/api/bookingdashboard/{id}", async (int id) =>
{
    try
    {
        var cDb = new ClaseDB<BookingDashboard>();
        var data = await cDb.GetAllAsync(@$"select
                                                b.BookingId,
	                                            b.LastETD,
	                                            b.LastETA,
	                                            ct.ContractType,
	                                            if(week(b.LastETD, 3) = 0,1,WEEK(b.LastETD,3)) as WK,
	                                            b.StatusBL,
	                                            b.PoRef,
	                                            sh.Shipper,
	                                            b.NroRequest,
	                                            b.NroBooking,
	                                            b.QtyContainers,
	                                            b.QtyContainersReal
                                            from Booking b
                                            left join Shippers sh on sh.ShipperId = b.ShipperId
                                            left join `ContractTypes` ct on ct.ContractTypeId = b.ContractTypeId
                                            WHERE b.SeasonId = {id}
                                            order by b.LastETD;");

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
    }
}
