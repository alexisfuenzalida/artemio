using Xunit;

namespace MinimalAPIArtemio.Tests;

public class BookingInsertSqlTests
{
    [Fact]
    public void BookingCreateSql_IncludesRequiredHsCodeColumn()
    {
        var source = System.IO.File.ReadAllText(
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "MinimalAPIArtemio", "Endpoints", "BookingEndpoints.cs")));

        var postBlock = source[source.IndexOf("app.MapPost(\"/api/booking\"", StringComparison.Ordinal)..];
        postBlock = postBlock[..postBlock.IndexOf("app.MapPut(\"/api/booking\"", StringComparison.Ordinal)];

        Assert.Contains("HsCode", postBlock);
        Assert.Contains("@HsCode", postBlock);
        Assert.Contains("data.HsCode ??=", postBlock);
    }

    [Fact]
    public void CopyBookingSql_ClearsPaymentCondition()
    {
        var source = System.IO.File.ReadAllText(
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "MinimalAPIArtemio", "Endpoints", "BookingEndpoints.cs")));

        var copyBlock = source[source.IndexOf("app.MapPost(\"/api/copybooking", StringComparison.Ordinal)..];
        copyBlock = copyBlock[..copyBlock.IndexOf("app.MapGet(\"/api/bookinghscode\"", StringComparison.Ordinal)];

        Assert.Contains("Humidity,0,StatusBL", copyBlock);
        Assert.DoesNotContain("Humidity,PaymentConditionId,StatusBL", copyBlock.Split("SELECT")[1]);
    }
}
