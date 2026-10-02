using MinimalAPIArtemio.Models;
using MySqlX.XDevAPI.Relational;

namespace MinimalAPIArtemio.Helpers
{
    public static class HelperTableHtml
    {
        public static string GetBookingTable(List<BookingSendModel> data)
        {
            string texto;
            string tabla;

            texto = "<!DOCTYPE html>";
            texto += @"<html lang=""en"">";
            texto += "<head>";
            texto += "<title>Page Title</title>";
            texto += "</head>";
            texto += "<body><p>Booking</p>";

            tabla = @"<table style=""border:2px solid #5EA3EF;padding:5px"">";

            tabla += "<thead>";
            tabla += @"<tr style=""background:#5EA3EF;color:white;font-size:13px;"">";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">ETD Week</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">ETD</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">Shipping Co.</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">SHIPPER</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">PO / REF.</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">VESSEL</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">VOYAGE</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">BOOKING</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">BL</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">CONTAINER</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">POL</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">POD</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">Place of Delivery</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">PAYMENT CONDITION</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">T°</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">CBM</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">Commodity</th>";
            tabla += @"<th style=""border:1px solid #5EA3EF;"">Suma de Q CTN</th>";
            tabla += "</tr>";
            tabla += "</thead>";

            tabla += "<tbody>";

            var sum = 0;
            foreach (var item in data)
            {
                var qty = GetContainerQuantity(item);
                tabla += "<tr>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.ETDWeek}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.ETD.Value.ToString("dd/MM/yyyy")}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.ShippingCo}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.Shipper}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.PoRef}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.Vessel}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.Voyage}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.NroBooking}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.BL}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.Container}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.POL}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.POD}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.DeliveryPlace}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.PaymentCondition}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.Temperature}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.CBM}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{item.Commodity}</td>";
                tabla += $@"<td style=""border:1px solid #5EA3EF;"">{qty}</td>";
                tabla += "</tr>";

                sum += qty;
            }

            tabla += "<tr>";
            tabla += @"<td style=""border:1px solid #5EA3EF;""  colspan=""17"">Total general</td>";
            tabla += $@"<td style=""border:1px solid #5EA3EF;"">{sum}</td>";
            tabla += "</tr>";

            tabla += "</tbody>";
            tabla += "</Table>";

            tabla += "</body>";
            tabla += "</html>";

            return tabla;
        }

        public static int GetContainerQuantity(BookingSendModel item)
        {
            if (item == null)
            {
                return 0;
            }

            if (item.QtyContainers > 0)
            {
                return item.QtyContainers;
            }

            return string.IsNullOrWhiteSpace(item.Container) ? 0 : 1;
        }

        public static (string Destination, string Comment) ResolveFreeDaysDestination(List<BookingSendDaysModel> data, string? selectedCountry = null)
        {
            var first = data?.FirstOrDefault();
            var country = string.IsNullOrWhiteSpace(selectedCountry) ? (first?.Country ?? "") : selectedCountry.Trim();
            var comment = first?.FreeDaysComment ?? "";
            var pods = data?
                .Select(x => x.POD ?? "")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList() ?? new List<string>();

            var allJakarta = pods.Count > 0 && pods.All(IsJakartaPod);

            if (allJakarta)
            {
                country = "Jakarta (Indonesia)";
                comment = "12 FREE DAYS MIX";
            }
            else if (string.IsNullOrWhiteSpace(comment) &&
                     (country.Contains("India", StringComparison.OrdinalIgnoreCase) ||
                      country.Contains("Indonesia", StringComparison.OrdinalIgnoreCase)))
            {
                comment = "12 FREE DAYS MIX";
            }

            return (country, comment);
        }

        private static bool IsJakartaPod(string pod)
        {
            return pod.Contains("Jakarta", StringComparison.OrdinalIgnoreCase)
                || pod.Contains("Djakarta", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetBookingFreeDaysTable(List<BookingSendDaysModel> data, string greeting, string country, string freeDaysComment)
        {
            var texto = "<!DOCTYPE html>";
            texto += @"<html lang=""en"">";
            texto += "<head>";
            texto += "<title>Booking Free Days</title>";
            texto += "</head>";
            texto += @"<body style=""font-family:Arial, Helvetica, sans-serif;"">";
            texto += $@"<p style=""margin-bottom:8px;"">Estimados, {greeting},</p>";

            var countryText = string.IsNullOrWhiteSpace(country) ? "-" : country;
            var destinationLine = string.IsNullOrWhiteSpace(freeDaysComment)
                ? $"Favor considerar sgte. listado para aplicación de días libres {countryText}"
                : $"Favor considerar sgte. listado para aplicación de días libres {countryText}, {freeDaysComment}";
            texto += $@"<p style=""margin-top:0;margin-bottom:12px;"">{destinationLine}</p>";

            texto += @"<table style=""border-collapse:collapse;border:1px solid #7DA16B;font-size:12px;"">";
            texto += "<thead>";
            texto += @"<tr style=""background:#3E5D2B;color:white;"">";
            texto += @"<th style=""border:1px solid #7DA16B;padding:4px 6px;"">Vessel</th>";
            texto += @"<th style=""border:1px solid #7DA16B;padding:4px 6px;"">Voyage</th>";
            texto += @"<th style=""border:1px solid #7DA16B;padding:4px 6px;"">NroBooking</th>";
            texto += @"<th style=""border:1px solid #7DA16B;padding:4px 6px;"">BL</th>";
            texto += @"<th style=""border:1px solid #7DA16B;padding:4px 6px;"">Container</th>";
            texto += @"<th style=""border:1px solid #7DA16B;padding:4px 6px;"">Shipper</th>";
            texto += @"<th style=""border:1px solid #7DA16B;padding:4px 6px;"">ETD</th>";
            texto += @"<th style=""border:1px solid #7DA16B;padding:4px 6px;"">POD</th>";
            texto += @"<th style=""border:1px solid #7DA16B;padding:4px 6px;"">PoRef</th>";
            texto += "</tr>";
            texto += "</thead>";

            texto += "<tbody>";
            foreach (var item in data)
            {
                texto += @"<tr style=""background:#DDECCF;color:#000;"">";
                texto += $@"<td style=""border:1px solid #7DA16B;padding:3px 6px;"">{item.Vessel}</td>";
                texto += $@"<td style=""border:1px solid #7DA16B;padding:3px 6px;"">{item.Voyage}</td>";
                texto += $@"<td style=""border:1px solid #7DA16B;padding:3px 6px;"">{item.NroBooking}</td>";
                texto += $@"<td style=""border:1px solid #7DA16B;padding:3px 6px;"">{item.BL}</td>";
                texto += $@"<td style=""border:1px solid #7DA16B;padding:3px 6px;"">{item.Container}</td>";
                texto += $@"<td style=""border:1px solid #7DA16B;padding:3px 6px;"">{item.Shipper}</td>";
                texto += $@"<td style=""border:1px solid #7DA16B;padding:3px 6px;"">{item.ETD.ToString("dd-MM-yyyy HH:mm")}</td>";
                texto += $@"<td style=""border:1px solid #7DA16B;padding:3px 6px;"">{item.POD}</td>";
                texto += $@"<td style=""border:1px solid #7DA16B;padding:3px 6px;"">{item.PoRef}</td>";
                texto += "</tr>";
            }

            texto += "</tbody>";
            texto += "</table>";
            texto += "</body>";
            texto += "</html>";

            return texto;
        }
    }
}
