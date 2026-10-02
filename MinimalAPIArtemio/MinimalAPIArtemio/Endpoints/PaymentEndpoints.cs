using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;

namespace MinimalAPIArtemio.Endpoints;

public static class PaymentEndpoints
{
    public static void MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/paymentsgrid/{seasonId:int}", async (int seasonId) =>
        {
            try
            {
                var db = new ClaseDB<PaymentGrid>();
                var data = await db.GetAllAsync(@"
                    SELECT
                        p.PaymentId,
                        p.Date,
                        p.Amount,
                        IFNULL(GROUP_CONCAT(DISTINCT i.Number ORDER BY i.Number SEPARATOR ', '), '') AS Invoices
                    FROM Payments p
                    INNER JOIN PaymentsDetails pd ON pd.PaymentId = p.PaymentId
                    INNER JOIN Invoices i ON i.InvoiceId = pd.InvoiceId
                    INNER JOIN InvoicesDetails d ON d.InvoiceId = i.InvoiceId
                    INNER JOIN Booking b ON b.BookingId = d.BookingId
                    WHERE b.SeasonId = @seasonId
                    GROUP BY p.PaymentId, p.Date, p.Amount
                    ORDER BY p.Date DESC, p.PaymentId DESC;",
                    new { seasonId });

                return Results.Ok(data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapGet("/api/payments", async () =>
        {
            try
            {
                var db = new ClaseDB<PaymentModel>();
                var data = await db.GetAllAsync("SELECT * FROM Payments ORDER BY Date DESC, PaymentId DESC;");
                return Results.Ok(data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapGet("/api/payments/{id:int}", async (int id) =>
        {
            try
            {
                var db = new ClaseDB<PaymentModel>();
                var data = await db.GetAsync("SELECT * FROM Payments WHERE PaymentId = @id;", new { id });
                return data == null
                    ? Results.NotFound(new { Message = "Not found" })
                    : Results.Ok(data);
            }
            catch (Exception ex)
            {
                return Results.NotFound(ex.Message);
            }
        });

        app.MapPost("/api/payments", async (PaymentModel data) =>
        {
            try
            {
                var db = new ClaseDB<PaymentModel>();
                data.PaymentId = await db.ExecuteInsertAsync(
                    data,
                    "INSERT INTO Payments (Date,Amount) VALUES (@Date,@Amount);");
                return Results.Created($"/api/payments/{data.PaymentId}", data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapPut("/api/payments", async (PaymentModel data) =>
        {
            try
            {
                var db = new ClaseDB<PaymentModel>();
                await db.ExcecuteQueryAsync(
                    data,
                    "UPDATE Payments SET Date=@Date,Amount=@Amount WHERE PaymentId=@PaymentId;");
                return Results.Ok(data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapDelete("/api/payments/{id:int}", async (int id) =>
        {
            try
            {
                var details = new ClaseDB<PaymentDetailModel>();
                await details.ExcecuteQueryAsync("DELETE FROM PaymentsDetails WHERE PaymentId=@id;", new { id });

                var db = new ClaseDB<PaymentModel>();
                await db.ExcecuteQueryAsync("DELETE FROM Payments WHERE PaymentId=@id;", new { id });
                return Results.Ok(new { Message = "Payment deleted" });
            }
            catch (Exception)
            {
                return Results.Ok(new { Message = "Error" });
            }
        });
    }
}
