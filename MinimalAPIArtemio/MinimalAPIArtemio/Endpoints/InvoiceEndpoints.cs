using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;

namespace MinimalAPIArtemio.Endpoints;

public static class InvoiceEndpoints
{
    public static void MapInvoiceEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/invoicesgrid/{seasonId:int}", async (int seasonId) =>
        {
            try
            {
                var db = new ClaseDB<InvoiceGrid>();
                var data = await db.GetAllAsync(@"
                    SELECT
                        inv.InvoiceId,
                        inv.Date,
                        inv.Type,
                        inv.Number,
                        inv.Amount,
                        inv.Observation,
                        IFNULL(GROUP_CONCAT(DISTINCT b.NroBooking ORDER BY b.NroBooking SEPARATOR ', '), '') AS Bookings
                    FROM Invoices inv
                    INNER JOIN InvoicesDetails d ON d.InvoiceId = inv.InvoiceId
                    INNER JOIN Booking b ON b.BookingId = d.BookingId
                    WHERE b.SeasonId = @seasonId
                    GROUP BY inv.InvoiceId, inv.Date, inv.Type, inv.Number, inv.Amount, inv.Observation
                    ORDER BY inv.Date DESC, inv.InvoiceId DESC;",
                    new { seasonId });

                return Results.Ok(data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapGet("/api/invoices", async () =>
        {
            try
            {
                var db = new ClaseDB<InvoiceModel>();
                var data = await db.GetAllAsync("SELECT * FROM Invoices ORDER BY Date DESC, InvoiceId DESC;");
                return Results.Ok(data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapGet("/api/invoices/season/{seasonId:int}/{paymentId:int}", async (int seasonId, int paymentId) =>
        {
            try
            {
                var db = new ClaseDB<InvoiceBalanceModel>();
                var data = await db.GetAllAsync(@"
                    SELECT
                        inv.InvoiceId,
                        inv.Number,
                        inv.Type,
                        inv.Amount,
                        IFNULL((SELECT SUM(pd.Amount) FROM PaymentsDetails pd
                                WHERE pd.InvoiceId = inv.InvoiceId AND pd.PaymentId <> @paymentId), 0) AS AmountPaid
                    FROM Invoices inv
                    INNER JOIN InvoicesDetails d ON d.InvoiceId = inv.InvoiceId
                    INNER JOIN Booking b ON b.BookingId = d.BookingId
                    WHERE b.SeasonId = @seasonId
                    GROUP BY inv.InvoiceId, inv.Number, inv.Type, inv.Amount
                    ORDER BY inv.Date DESC;",
                    new { seasonId, paymentId });

                return Results.Ok(data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapGet("/api/invoices/{id:int}", async (int id) =>
        {
            try
            {
                var db = new ClaseDB<InvoiceModel>();
                var data = await db.GetAsync("SELECT * FROM Invoices WHERE InvoiceId = @id;", new { id });
                return data == null
                    ? Results.NotFound(new { Message = "Not found" })
                    : Results.Ok(data);
            }
            catch (Exception ex)
            {
                return Results.NotFound(ex.Message);
            }
        });

        app.MapPost("/api/invoices", async (InvoiceModel data) =>
        {
            try
            {
                data.Observation ??= "";
                data.Number ??= "";
                data.Type ??= "";
                var db = new ClaseDB<InvoiceModel>();
                data.InvoiceId = await db.ExecuteInsertAsync(
                    data,
                    "INSERT INTO Invoices (Date,Type,Number,Amount,Observation) VALUES (@Date,@Type,@Number,@Amount,@Observation);");
                return Results.Created($"/api/invoices/{data.InvoiceId}", data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapPut("/api/invoices", async (InvoiceModel data) =>
        {
            try
            {
                data.Observation ??= "";
                data.Number ??= "";
                data.Type ??= "";
                var db = new ClaseDB<InvoiceModel>();
                await db.ExcecuteQueryAsync(
                    data,
                    "UPDATE Invoices SET Date=@Date,Type=@Type,Number=@Number,Amount=@Amount,Observation=@Observation WHERE InvoiceId=@InvoiceId;");
                return Results.Ok(data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapDelete("/api/invoices/{id:int}", async (int id) =>
        {
            try
            {
                var details = new ClaseDB<InvoiceDetailModel>();
                await details.ExcecuteQueryAsync("DELETE FROM InvoicesDetails WHERE InvoiceId=@id;", new { id });

                var db = new ClaseDB<InvoiceModel>();
                await db.ExcecuteQueryAsync("DELETE FROM Invoices WHERE InvoiceId=@id;", new { id });
                return Results.Ok(new { Message = "Invoice deleted" });
            }
            catch (Exception)
            {
                return Results.Ok(new { Message = "Error" });
            }
        });
    }
}
