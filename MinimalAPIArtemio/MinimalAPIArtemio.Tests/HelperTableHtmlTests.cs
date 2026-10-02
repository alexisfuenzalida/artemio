using MinimalAPIArtemio.Helpers;
using MinimalAPIArtemio.Models;
using Xunit;

namespace MinimalAPIArtemio.Tests;

public class HelperTableHtmlTests
{
    [Fact]
    public void GetContainerQuantity_UsesQtyContainersFromBooking()
    {
        var item = new BookingSendModel
        {
            QtyContainers = 5,
            Container = "CTN1"
        };

        Assert.Equal(5, HelperTableHtml.GetContainerQuantity(item));
    }

    [Fact]
    public void GetContainerQuantity_FallsBackToOneWhenContainerExists()
    {
        var item = new BookingSendModel
        {
            QtyContainers = 0,
            Container = "MSCU1234567"
        };

        Assert.Equal(1, HelperTableHtml.GetContainerQuantity(item));
    }

    [Fact]
    public void GetBookingTable_WritesQtyContainersAndTotal()
    {
        var html = HelperTableHtml.GetBookingTable(new List<BookingSendModel>
        {
            new()
            {
                ETDWeek = 12,
                ETD = new DateTime(2026, 3, 20),
                ShippingCo = "MSC",
                Shipper = "Shipper A",
                PoRef = "PO-1",
                Vessel = "Vessel",
                Voyage = "001",
                NroBooking = "BK-100",
                BL = "BL123",
                Container = "MSCU1,MSCU2,MSCU3",
                POL = "Valparaiso",
                POD = "Shanghai",
                DeliveryPlace = "Shanghai",
                PaymentCondition = "Prepaid",
                Temperature = -1,
                CBM = "67",
                Commodity = "Cherry",
                QtyContainers = 3
            },
            new()
            {
                ETDWeek = 12,
                ETD = new DateTime(2026, 3, 21),
                ShippingCo = "Hapag",
                Shipper = "Shipper B",
                PoRef = "PO-2",
                Vessel = "Vessel 2",
                Voyage = "002",
                NroBooking = "BK-200",
                BL = "BL999",
                Container = "TCLU1",
                POL = "San Antonio",
                POD = "Rotterdam",
                DeliveryPlace = "Rotterdam",
                PaymentCondition = "Collect",
                Temperature = 0,
                CBM = "12",
                Commodity = "Grape",
                QtyContainers = 2
            }
        });

        Assert.Contains(">3<", html);
        Assert.Contains(">2<", html);
        Assert.Contains("BL123", html);
        Assert.Contains("BL999", html);
        Assert.Contains(">5<", html);
        Assert.DoesNotContain("colspan=\"16\"", html);
        Assert.Contains("colspan=\"17\"", html);
    }
}
