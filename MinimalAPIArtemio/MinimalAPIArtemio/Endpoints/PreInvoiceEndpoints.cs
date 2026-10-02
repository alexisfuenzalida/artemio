using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;

namespace MinimalAPIArtemio.Endpoints;

public static class PreInvoiceEndpoints
{
    public static void MapPreInvoiceEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/preinvoices/booking/{bookingId:int}", async (int bookingId) =>
        {
            var db = new ClaseDB<PreInvoiceModel>();
            var data = await db.GetAllAsync("SELECT * FROM PreInvoices WHERE BookingId = @bookingId;", new { bookingId });
            return Results.Ok(data);
        });

        app.MapGet("/api/preinvoices/season/{seasonId:int}/type/{type}", async (int seasonId, string type, int invoiceId = 0) =>
        {
            var db = new ClaseDB<PreInvoiceModel>();
            var data = await db.GetAllAsync(@"
                SELECT
                    p.PreInvoiceId,
                    p.Type,
                    p.Amount,
                    p.BookingId,
                    IFNULL(b.NroBooking, '') AS NroBooking
                FROM PreInvoices p
                INNER JOIN Booking b ON b.BookingId = p.BookingId
                WHERE p.Type = @type
                  AND b.SeasonId = @seasonId
                  AND p.BookingId NOT IN (
                      SELECT d.BookingId
                      FROM InvoicesDetails d
                      INNER JOIN Invoices i ON i.InvoiceId = d.InvoiceId
                      WHERE i.Type = @type AND i.InvoiceId <> @invoiceId
                  )
                ORDER BY b.NroBooking;",
                new { seasonId, type, invoiceId });
            return Results.Ok(data);
        });

        app.MapPost("/api/preinvoices", async (PreInvoiceModel data) =>
        {
            var db = new ClaseDB<PreInvoiceModel>();
            data.PreInvoiceId = await db.ExecuteInsertAsync(data, "INSERT INTO PreInvoices (Type,Amount,BookingId) VALUES (@Type,@Amount,@BookingId);");
            return Results.Created($"/api/preinvoices/{data.PreInvoiceId}", data);
        });

        app.MapPut("/api/preinvoices", async (PreInvoiceModel data) =>
        {
            var db = new ClaseDB<PreInvoiceModel>();
            await db.ExcecuteQueryAsync(data, "UPDATE PreInvoices SET Type=@Type,Amount=@Amount,BookingId=@BookingId WHERE PreInvoiceId=@PreInvoiceId;");
            return Results.Ok(data);
        });

        app.MapDelete("/api/preinvoices/{bookingId:int}/{type}", async (int bookingId, string type) =>
        {
            var db = new ClaseDB<PreInvoiceModel>();
            await db.ExcecuteQueryAsync("DELETE FROM PreInvoices WHERE BookingId=@bookingId AND Type=@type;", new { bookingId, type });
            return Results.Ok(new { Message = "PreInvoice deleted" });
        });

        app.MapDelete("/api/preinvoices/booking/{bookingId:int}", async (int bookingId) =>
        {
            var db = new ClaseDB<PreInvoiceModel>();
            await db.ExcecuteQueryAsync("DELETE FROM PreInvoices WHERE BookingId=@bookingId;", new { bookingId });
            return Results.Ok(new { Message = "PreInvoices deleted" });
        });
    }
}
