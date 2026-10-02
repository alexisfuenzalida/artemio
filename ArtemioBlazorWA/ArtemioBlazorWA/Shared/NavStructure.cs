using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;

namespace ArtemioBlazorWA.Shared;

public record NavItem(string Label, string Href, string Icon, NavLinkMatch Match = NavLinkMatch.Prefix);

public record NavGroup(string Title, string Icon, List<NavItem> Items);

public static class NavStructure
{
    public static readonly List<NavGroup> Groups = new()
    {
        new NavGroup("Home", Icons.Material.Filled.Home, new()
        {
            new("Booking", "/booking", Icons.Material.Filled.BookOnline, NavLinkMatch.All),
            new("Contracts", "/contracts", Icons.Material.Filled.Description),
            new("Itineraries", "/itineraries", Icons.Material.Filled.Route),
            new("Incidences", "/incidences", Icons.Material.Filled.ReportProblem),
            new("TTA", "/tta", Icons.Material.Filled.FactCheck),
        }),
        new NavGroup("Accounting", Icons.Material.Filled.Receipt, new()
        {
            new("Invoices", "/invoices", Icons.Material.Filled.ReceiptLong),
            new("Payments", "/payments", Icons.Material.Filled.Payments),
        }),
        new NavGroup("Geography", Icons.Material.Filled.Public, new()
        {
            new("Countries", "/countries", Icons.Material.Filled.Flag),
            new("Cities", "/cities", Icons.Material.Filled.LocationCity),
            new("Ports", "/ports", Icons.Material.Filled.Anchor),
        }),
        new NavGroup("Third Parties", Icons.Material.Filled.Groups, new()
        {
            new("Cargo Agents", "/agents", Icons.Material.Filled.LocalShipping, NavLinkMatch.All),
            new("Shipping Agents", "/agentsembarques", Icons.Material.Filled.DirectionsBoat, NavLinkMatch.All),
            new("Customs Agents", "/agentsaduanas", Icons.Material.Filled.Gavel, NavLinkMatch.All),
            new("Shippers", "/shippers", Icons.Material.Filled.Outbox),
            new("Consignees", "/consignees", Icons.Material.Filled.Inbox),
            new("Intermediaries", "/intermediaries", Icons.Material.Filled.CompareArrows),
            new("Notifies", "/notifies", Icons.Material.Filled.NotificationsActive),
        }),
        new NavGroup("Fleet & Equipment", Icons.Material.Filled.DirectionsBoatFilled, new()
        {
            new("Vessels", "/vessels", Icons.Material.Filled.DirectionsBoat),
            new("Containers Type", "/containerstype", Icons.Material.Filled.ViewInAr),
            new("Transports", "/transports", Icons.Material.Filled.LocalShipping),
            new("Shipping Companies", "/transportcompanies", Icons.Material.Filled.Business),
            new("Status Containers", "/statuscontainers", Icons.Material.Filled.Inventory),
        }),
        new NavGroup("Commercial", Icons.Material.Filled.Handshake, new()
        {
            new("Incoterms", "/incoterms", Icons.Material.Filled.Rule),
            new("Payment Conditions", "/paymentconditions", Icons.Material.Filled.Payments),
            new("Contract Types", "/contracttypes", Icons.Material.Filled.Category),
            new("Cost Items", "/costitems", Icons.Material.Filled.AttachMoney),
            new("Services", "/services", Icons.Material.Filled.MiscellaneousServices),
        }),
        new NavGroup("Operations", Icons.Material.Filled.Tune, new()
        {
            new("Free Days", "/freedays", Icons.Material.Filled.EventAvailable),
            new("Stacking", "/stacking", Icons.Material.Filled.Layers),
            new("Seasons", "/seasons", Icons.Material.Filled.CalendarMonth),
            new("TT", "/tts", Icons.Material.Filled.Schedule),
            new("BL Issued", "/blissueds", Icons.Material.Filled.AssignmentTurnedIn),
            new("Depots", "/warehouses", Icons.Material.Filled.Warehouse),
        }),
        new NavGroup("Administration", Icons.Material.Filled.AdminPanelSettings, new()
        {
            new("Users", "/users", Icons.Material.Filled.ManageAccounts),
        }),
    };

    public static string? GetTitle(string relativePath)
    {
        var path = "/" + relativePath.TrimStart('/');

        if (string.Equals(path, "/dashboard", StringComparison.OrdinalIgnoreCase))
        {
            return "Dashboard";
        }

        foreach (var group in Groups)
        {
            foreach (var item in group.Items)
            {
                if (string.Equals(item.Href, path, StringComparison.OrdinalIgnoreCase))
                {
                    return item.Label;
                }
            }
        }

        return null;
    }
}
