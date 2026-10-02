from __future__ import annotations

import re
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1] / "MinimalAPIArtemio"
PROGRAM = ROOT / "Program.cs"
ENDPOINTS = ROOT / "Endpoints"

SKIP_REGIONS = {"Login", "Users"}

REGION_CLASS = {
    "Agents": "AgentEndpoints",
    "Agents Aduanas": "AgentAduanaEndpoints",
    "Agents Embarque": "AgentEmbarqueEndpoints",
    "BlIssued": "BlIssuedEndpoints",
    "Cities": "CityEndpoints",
    "Commodities": "CommodityEndpoints",
    "Consignees": "ConsigneeEndpoints",
    "Container Type": "ContainerTypeEndpoints",
    "Cost Item": "CostItemEndpoints",
    "Contract Type": "ContractTypeEndpoints",
    "Countries": "CountryEndpoints",
    "FreeDays": "FreeDaysEndpoints",
    "Incidences Type": "IncidenceTypeEndpoints",
    "Incoterms": "IncotermEndpoints",
    "Intermediaries": "IntermediaryEndpoints",
    "Invoices": "InvoiceEndpoints",
    "PreInvoices": "PreInvoiceEndpoints",
    "Involved": "InvolvedEndpoints",
    "Itineraries": "ItineraryEndpoints",
    "ItinerariesDetails": "ItineraryDetailEndpoints",
    "ItinerariesETD": "ItineraryEtdEndpoints",
    "ItinerariesTransportCompanies": "ItineraryTransportCompanyEndpoints",
    "Notifies": "NotifyEndpoints",
    "PaymentConditions": "PaymentConditionEndpoints",
    "Ports": "PortEndpoints",
    "Seasons": "SeasonEndpoints",
    "Services": "ServiceEndpoints",
    "Shippers": "ShipperEndpoints",
    "Stacking": "StackingEndpoints",
    "StatusContainers": "StatusContainerEndpoints",
    "Transports": "TransportEndpoints",
    "Transport Companies": "TransportCompanyEndpoints",
    "Tta": "TtaEndpoints",
    "TTs": "TtEndpoints",
    "TTsDetails": "TtDetailEndpoints",
    "Vessels": "VesselEndpoints",
    "Warehouses": "WarehouseEndpoints",
    "Booking": "BookingEndpoints",
    "BookingDtl": "BookingDetailEndpoints",
    "Booking Incidences": "BookingIncidenceEndpoints",
    "Booking ETA": "BookingEtaEndpoints",
    "Booking POD": "BookingPodEndpoints",
    "Send Booking": "SendBookingEndpoints",
    "Booking Stacking": "BookingStackingEndpoints",
    "Booking Send Free Days": "BookingSendDaysEndpoints",
    "Contract": "ContractEndpoints",
    "ContractDtl": "ContractDetailEndpoints",
    "ContractsConditions": "ContractConditionEndpoints",
    "Dashboard": "DashboardEndpoints",
    "ReportsExcel": "ReportEndpoints",
}

USINGS = """using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Helpers;
using MinimalAPIArtemio.Models;
using MinimalAPIArtemio.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
"""


def transform(body: str) -> str:
    body = re.sub(
        r"await cDb\.ExcecuteQueryAsync\(([^,]+), (\"INSERT[^\"]+\")\);\s*\n\s*\1\.(\w+) = await cDb\.GetLastIdAsync\(\"SELECT MAX\([^\"]+\"\);",
        r"\1.\3 = await cDb.ExecuteInsertAsync(\1, \2);",
        body,
    )
    body = re.sub(
        r"await cDb\.ExcecuteQueryAsync\(([^,]+), (\"INSERT[^\"]+\")\);\s*\n\s*var newId = await cDb\.GetLastIdAsync\(\"SELECT MAX\([^\"]+\"\);",
        r"var newId = await cDb.ExecuteInsertAsync(\1, \2);",
        body,
    )

    replacements = [
        (
            'await cDb.ExcecuteQueryAsync($"DELETE FROM PreInvoices WHERE BookingId={bookingId} AND Type=\'{type}\';");',
            'await cDb.ExcecuteQueryAsync("DELETE FROM PreInvoices WHERE BookingId=@bookingId AND Type=@type;", new { bookingId, type });',
        ),
        (
            'var data = await cDb.GetAllAsync($"SELECT * FROM BookingIncidences WHERE BookingId = {id} AND Container = \'{container}\';");',
            'var data = await cDb.GetAllAsync("SELECT * FROM BookingIncidences WHERE BookingId = @id AND Container = @container;", new { id, container });',
        ),
        (
            'await cDb.ExcecuteQueryAsync($"DELETE FROM BookingIncidences WHERE BookingId={id} AND Container=\'{container}\';");',
            'await cDb.ExcecuteQueryAsync("DELETE FROM BookingIncidences WHERE BookingId=@id AND Container=@container;", new { id, container });',
        ),
    ]
    for old, new in replacements:
        body = body.replace(old, new)

    body = re.sub(
        r'AND b\.Voyage = \'\{item\.Voyage\}\';',
        "AND b.Voyage = @Voyage;",
        body,
    )
    if "AND b.Voyage = @Voyage;" in body and "ExcecuteQueryAsync(@$\"" in body:
        body = body.replace(
            'await cDb.ExcecuteQueryAsync(@$"UPDATE Booking b SET b.StackingEmail = 1 WHERE b.`TransportCompanyId` = {item.TransportCompanyId} AND b.`VesselId` = {item.VesselId} AND b.Voyage = @Voyage;");',
            'await cDb.ExcecuteQueryAsync("UPDATE Booking b SET b.StackingEmail = 1 WHERE b.`TransportCompanyId` = @TransportCompanyId AND b.`VesselId` = @VesselId AND b.Voyage = @Voyage;", item);',
        )

    body = re.sub(
        r'string tenantId = "[^"]+";\s*string clientId = "[^"]+";\s*string clientSecret = "[^"]+";\s*var sender = "[^"]+";',
        "var tenantId = graphOptions.Value.TenantId;\n        var clientId = graphOptions.Value.ClientId;\n        var clientSecret = graphOptions.Value.ClientSecret;\n        var sender = graphOptions.Value.Sender;",
        body,
    )
    body = body.replace(
        "app.MapPost(\"/api/sendbooking\",async (BookingIdModel body) =>",
        "app.MapPost(\"/api/sendbooking\", async (BookingIdModel body, IOptions<GraphOptions> graphOptions) =>",
    )
    body = body.replace(
        "app.MapPost(\"/api/bookingsenddays\", async (BookingSendDaysRequestModel body) =>",
        "app.MapPost(\"/api/bookingsenddays\", async (BookingSendDaysRequestModel body, IOptions<GraphOptions> graphOptions) =>",
    )

    if 'app.MapGet("/api/seasons"' in body:
        body = body.replace(
            'app.MapGet("/api/seasons", async () =>',
            'app.MapGet("/api/seasons", async () =>',
        )
        # add AllowAnonymous after the first seasons endpoint closing
        body = re.sub(
            r'(app\.MapGet\("/api/seasons", async \(\) =>\s*\{.*?\}\);)',
            r"\1.AllowAnonymous()",
            body,
            count=1,
            flags=re.S,
        )

    return body


def to_class_name(region: str) -> str:
    return REGION_CLASS.get(region, re.sub(r"[^A-Za-z0-9]", "", region) + "Endpoints")


def main() -> None:
    text = PROGRAM.read_text(encoding="latin-1")
    pattern = re.compile(r"#region (?P<name>[^\n]+)\r?\n(?P<body>.*?)#endregion", re.S)
    ENDPOINTS.mkdir(exist_ok=True)

    generated = []
    for match in pattern.finditer(text):
        name = match.group("name").strip()
        if name in SKIP_REGIONS:
            continue
        body = transform(match.group("body").rstrip() + "\n")
        class_name = to_class_name(name)
        if not class_name.endswith("Endpoints") or len(class_name) > 80:
            raise SystemExit(f"Invalid class name for region {name!r}: {class_name[:80]!r}")
        method = f"Map{class_name.replace('Endpoints', '')}Endpoints"
        if not method.endswith("Endpoints"):
            method = f"Map{class_name}"
        # MapAgentEndpoints from AgentEndpoints
        method = "Map" + class_name
        file_path = ENDPOINTS / f"{class_name}.cs"
        content = f"""{USINGS}
namespace MinimalAPIArtemio.Endpoints;

public static class {class_name}
{{
    public static void {method}(this IEndpointRouteBuilder app)
    {{
{body}    }}
}}
"""
        file_path.write_text(content, encoding="utf-8")
        generated.append((class_name, method))
        print(f"wrote {file_path.name}")

    ext_path = ENDPOINTS / "EndpointRouteBuilderExtensions.cs"
    calls = "\n        ".join(f"app.{method}();" for _, method in generated)
    ext_path.write_text(
        f"""namespace MinimalAPIArtemio.Endpoints;

public static class EndpointRouteBuilderExtensions
{{
    public static void MapArtemioEndpoints(this IEndpointRouteBuilder app)
    {{
        app.MapAuthEndpoints();
        app.MapUserEndpoints();
        app.MapHealthEndpoints();
        {calls}
    }}
}}
""",
        encoding="utf-8",
    )
    print(f"generated {len(generated)} endpoint files")


if __name__ == "__main__":
    main()
