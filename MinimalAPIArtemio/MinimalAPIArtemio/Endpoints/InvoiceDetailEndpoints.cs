using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;

namespace MinimalAPIArtemio.Endpoints;

public static class InvoiceDetailEndpoints
{
    public static void MapInvoiceDetailEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/invoicesdetails/{id:int}", async (int id) =>
        {
            try
            {
                var db = new ClaseDB<InvoiceDetailModel>();
                var data = await db.GetAllAsync(@"
                    SELECT
                        d.Amount,
                        d.BookingId,
                        d.InvoiceId,
                        IFNULL(b.NroBooking, '') AS NroBooking
                    FROM InvoicesDetails d
                    LEFT JOIN Booking b ON b.BookingId = d.BookingId
                    WHERE d.InvoiceId = @id
                    ORDER BY b.NroBooking;",
                    new { id });
                return Results.Ok(data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapPost("/api/invoicesdetails", async (InvoiceDetailModel data) =>
        {
            try
            {
                var db = new ClaseDB<InvoiceDetailModel>();
                await db.ExcecuteQueryAsync(
                    data,
                    "INSERT INTO InvoicesDetails (Amount,BookingId,InvoiceId) VALUES (@Amount,@BookingId,@InvoiceId);");
                return Results.Created($"/api/invoicesdetails/{data.InvoiceId}", data);
            }
            catch (Exception)
            {
                return Results.BadRequest(new { Message = "Error" });
            }
        });

        app.MapDelete("/api/invoicesdetails/{id:int}", async (int id) =>
        {
            try
            {
                var db = new ClaseDB<InvoiceDetailModel>();
                await db.ExcecuteQueryAsync("DELETE FROM InvoicesDetails WHERE InvoiceId=@id;", new { id });
                return Results.Ok(new { Message = "Invoice details deleted" });
            }
            catch (Exception)
            {
                return Results.Ok(new { Message = "Error" });
            }
        });
    }
}
