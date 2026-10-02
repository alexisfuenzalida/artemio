using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;

namespace MinimalAPIArtemio.Endpoints;

public static class PaymentDetailEndpoints
{
    public static void MapPaymentDetailEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/paymentsdetails/{id:int}", async (int id) =>
        {
            try
            {
                var db = new ClaseDB<PaymentDetailModel>();
                var data = await db.GetAllAsync(@"
                    SELECT
                        pd.Amount,
                        pd.InvoiceId,
                        pd.PaymentId,
                        IFNULL(i.Number, '') AS InvoiceNumber
                    FROM PaymentsDetails pd
                    LEFT JOIN Invoices i ON i.InvoiceId = pd.InvoiceId
                    WHERE pd.PaymentId = @id
                    ORDER BY i.Number;",
                    new { id });
                return Results.Ok(data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapPost("/api/paymentsdetails", async (PaymentDetailModel data) =>
        {
            try
            {
                var db = new ClaseDB<PaymentDetailModel>();
                await db.ExcecuteQueryAsync(
                    data,
                    "INSERT INTO PaymentsDetails (Amount,InvoiceId,PaymentId) VALUES (@Amount,@InvoiceId,@PaymentId);");
                return Results.Created($"/api/paymentsdetails/{data.PaymentId}", data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapDelete("/api/paymentsdetails/{id:int}", async (int id) =>
        {
            try
            {
                var db = new ClaseDB<PaymentDetailModel>();
                await db.ExcecuteQueryAsync("DELETE FROM PaymentsDetails WHERE PaymentId=@id;", new { id });
                return Results.Ok(new { Message = "Payment details deleted" });
            }
            catch (Exception)
            {
                return Results.Ok(new { Message = "Error" });
            }
        });
    }
}
