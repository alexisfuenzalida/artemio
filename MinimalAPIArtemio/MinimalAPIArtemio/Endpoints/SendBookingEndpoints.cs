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

public static class SendBookingEndpoints
{
    public static void MapSendBookingEndpoints(this IEndpointRouteBuilder app)
    {
app.MapPost("/api/sendbooking", async (BookingIdModel body, IOptions<GraphOptions> graphOptions) =>
{
    try
    {
        var tenantId = graphOptions.Value.TenantId;
        var clientId = graphOptions.Value.ClientId;
        var clientSecret = graphOptions.Value.ClientSecret;
        var sender = graphOptions.Value.Sender;

        if (string.IsNullOrWhiteSpace(tenantId) ||
            string.IsNullOrWhiteSpace(clientId) ||
            string.IsNullOrWhiteSpace(clientSecret) ||
            string.IsNullOrWhiteSpace(sender))
        {
            return Results.BadRequest(new { Message = "Email service is not configured (Graph settings)" });
        }

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

        var graph = new GraphServiceClient(credential, new[] { "https://graph.microsoft.com/.default" });

        var toRecipients = new List<Recipient>();
        toRecipients.Add(new Recipient
        {
            EmailAddress = new EmailAddress
            {
                Address = body.Email
            }
        });

        var x = string.Join(",", body.Id.ToArray());

        var cDb = new ClaseDB<BookingSendModel>();
        var data = await cDb.GetAllAsync(@$"SELECT
                                                IF(WEEK(IFNULL(b.`LastETD`,DATE('1900-01-01')),3) = 0,1,WEEK(IFNULL(b.`LastETD`,DATE('1900-01-01')),3)) AS ETDWeek,
	                                            IFNULL(b.`LastETD`,DATE('1900-01-01')) AS ETD,
	                                            tc.`TransportCompany` AS ShippingCo,
	                                            sh.`Shipper`,
	                                            b.`PoRef`,
	                                            ve.`Vessel`,
	                                            b.`Voyage`,
	                                            b.`NroBooking`,
	                                            pol.`Port` AS POL,
	                                            pod.`Port` AS POD,
	                                            b.`DeliveryPlace`,
	                                            pc.`PaymentCondition`,
	                                            b.`Temperature`,
	                                            b.`CBM`,
	                                            b.`QtyContainers`,
                                                b.NroRequest,
                                                IFNULL((SELECT NULLIF(GROUP_CONCAT(DISTINCT NULLIF(TRIM(bd.Container), '')), '') FROM BookingDetails bd WHERE bd.BookingId = b.BookingId), '') AS Container,
                                                IFNULL((SELECT NULLIF(GROUP_CONCAT(DISTINCT NULLIF(TRIM(bd.BL), '')), '') FROM BookingDetails bd WHERE bd.BookingId = b.BookingId), '') AS BL,
                                                IFNULL(co.`Commodity`, '') AS Commodity
                                            FROM Booking b
                                            LEFT JOIN `TransportCompanies` tc ON tc.`TransportCompanyId` = b.`TransportCompanyId`
                                            LEFT JOIN `Shippers` sh ON sh.`ShipperId` = b.`ShipperId`
                                            LEFT JOIN `Vessels` ve ON ve.`VesselId` = b.`VesselId`
                                            LEFT JOIN `Ports` pol ON pol.`PortId` = b.`POLId`
                                            LEFT JOIN `Ports` pod ON pod.`PortId` = b.`LastPODId`
                                            LEFT JOIN `PaymentConditions` pc ON pc.`PaymentConditionId` = b.`PaymentConditionId`
                                            LEFT JOIN `Commodities` co ON co.`CommodityId` = b.`CommodityId`
                                            WHERE b.BookingId in ({x});");

        string NroRequest = "", Container = "", BL ="", Vessel = "", POL = "", POD = "", PoRef = "";

        foreach (var item in data)
        {
            NroRequest = item.NroRequest;
            Container = item.Container;
            BL = item.BL;
            Vessel = item.Vessel;
            POL = item.POL;
            POD = item.POD;
            PoRef = item.PoRef;
        }


        await graph.Users[sender].SendMail.PostAsync(new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
        {
            Message = new Message
            {
                Subject = $"SOL. BOOKING: {Vessel}/{POL}/{POD}/{PoRef}",
                Body = new ItemBody
                { 
                    ContentType = BodyType.Html, 
                    Content = HelperTableHtml.GetBookingTable(data)
                },
                ToRecipients = toRecipients
            },
            SaveToSentItems = true
        });

        return Results.Ok(new
        {
            Message = "Booking Send"
        });
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
