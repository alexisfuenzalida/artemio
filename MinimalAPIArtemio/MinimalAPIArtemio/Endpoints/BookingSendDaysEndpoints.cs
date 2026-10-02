using Dapper;
using Microsoft.Extensions.Options;
using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Helpers;
using MinimalAPIArtemio.Models;
using MinimalAPIArtemio.Options;
using MinimalAPIArtemio.Services;

namespace MinimalAPIArtemio.Endpoints;

public static class BookingSendDaysEndpoints
{
    public static void MapBookingSendDaysEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookingsenddaysgrid", async () =>
        {
            try
            {
                var db = new ClaseDB<BookingGrid>();
                var data = await db.GetAllAsync(@"SELECT
                                                    b.BookingId,
                                                    b.NroBooking,
                                                    b.NroRequest,
                                                    b.PoRef,
                                                    b.Voyage,
                                                    IFNULL(b.LastETD, DATE('1900-01-01')) AS ETD,
                                                    ve.Vessel,
                                                    tc.TransportCompany AS ShippingCompany,
                                                    pol.Port AS POL,
                                                    pod.Port AS POD,
                                                    sh.Shipper,
                                                    bd.BL,
                                                    bd.Container
                                                FROM BookingDetails bd
                                                INNER JOIN Booking b ON b.BookingId = bd.BookingId
                                                LEFT JOIN Vessels ve ON ve.VesselId = b.VesselId
                                                LEFT JOIN TransportCompanies tc ON tc.TransportCompanyId = b.TransportCompanyId
                                                LEFT JOIN Ports pol ON pol.PortId = b.POLId
                                                LEFT JOIN Ports pod ON pod.PortId = b.LastPODId
                                                LEFT JOIN Shippers sh ON sh.ShipperId = b.ShipperId
                                                WHERE IFNULL(bd.BL,'') <> '' AND IFNULL(bd.BL,'') <> '-' AND IFNULL(bd.SendFreeDays,0) <> 1
                                                ORDER BY b.LastETD DESC, bd.BL;");
                return Results.Ok(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Results.BadRequest(new { Message = ex.Message });
            }
        });

        app.MapPost("/api/bookingsenddays", async (BookingSendDaysRequestModel body, GraphMailService mail, IOptions<GraphOptions> graphOptions) =>
        {
            try
            {
                var items = (body.Items ?? new List<BookingSendDaysItemModel>())
                    .Select(x => new BookingSendDaysItemModel
                    {
                        BookingId = x.BookingId,
                        BL = (x.BL ?? "").Trim()
                    })
                    .Where(x => x.BookingId > 0 && !string.IsNullOrWhiteSpace(x.BL) && x.BL != "-")
                    .GroupBy(x => (x.BookingId, BL: x.BL.ToLowerInvariant()))
                    .Select(g => g.First())
                    .ToList();

                if (items.Count == 0 || string.IsNullOrWhiteSpace(body.Email))
                {
                    return Results.BadRequest(new { Message = "Select at least one BL and a valid email" });
                }

                var parameters = new DynamicParameters();
                var pairFilters = new List<string>();
                for (var i = 0; i < items.Count; i++)
                {
                    pairFilters.Add($"(b.BookingId = @bookingId{i} AND bd.BL = @bl{i})");
                    parameters.Add($"bookingId{i}", items[i].BookingId);
                    parameters.Add($"bl{i}", items[i].BL);
                }

                var countryFilter = "";
                if (body.CountryId > 0)
                {
                    countryFilter = " AND con.CountryId = @countryId";
                    parameters.Add("countryId", body.CountryId);
                }

                var db = new ClaseDB<BookingSendDaysModel>();
                var data = await db.GetAllAsync($@"SELECT
                                                    con.Country,
                                                    ve.Vessel,
                                                    b.Voyage,
                                                    b.NroBooking,
                                                    pod.Port AS POD,
                                                    b.PoRef,
                                                    bd.BL,
                                                    bd.Container,
                                                    sh.Shipper,
                                                    IFNULL(con.FreeDaysComment, '') AS FreeDaysComment,
                                                    IFNULL(b.LastETD, DATE('1900-01-01')) AS ETD
                                                FROM BookingDetails bd
                                                INNER JOIN Booking b ON b.BookingId = bd.BookingId
                                                LEFT JOIN Vessels ve ON ve.VesselId = b.VesselId
                                                LEFT JOIN Ports pod ON pod.PortId = b.LastPODId
                                                LEFT JOIN Countries con ON con.CountryId = pod.CountryId
                                                LEFT JOIN Shippers sh ON sh.ShipperId = b.ShipperId
                                                WHERE ({string.Join(" OR ", pairFilters)}){countryFilter};",
                    parameters);

                if (data == null || data.Count == 0)
                {
                    return Results.BadRequest(new { Message = "No booking details found for the selected BL" });
                }

                var graph = graphOptions.Value;
                if (string.IsNullOrWhiteSpace(graph.TenantId) ||
                    string.IsNullOrWhiteSpace(graph.ClientId) ||
                    string.IsNullOrWhiteSpace(graph.ClientSecret) ||
                    string.IsNullOrWhiteSpace(graph.Sender))
                {
                    return Results.BadRequest(new { Message = "Email service is not configured (Graph settings)" });
                }

                var (destination, freeDaysComment) = HelperTableHtml.ResolveFreeDaysDestination(data, body.Country);
                var html = HelperTableHtml.GetBookingFreeDaysTable(data, "", destination, freeDaysComment);
                await mail.SendHtmlAsync(body.Email, "Free Days", html);

                var updateDb = new ClaseDB<BookingSendDaysModel>();
                await updateDb.ExcecuteQueryAsync($@"UPDATE BookingDetails bd
                                                      INNER JOIN Booking b ON b.BookingId = bd.BookingId
                                                      SET bd.SendFreeDays = 1
                                                      WHERE {string.Join(" OR ", pairFilters)};",
                    parameters);

                return Results.Ok(new { Message = "Booking send days sent" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Results.BadRequest(new { Message = ex.InnerException?.Message ?? ex.Message });
            }
        });
    }
}
