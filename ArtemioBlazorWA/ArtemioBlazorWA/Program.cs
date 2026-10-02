using ArtemioBlazorWA;
using ArtemioBlazorWA.Auth;
using ArtemioBlazorWA.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<AuthTokenStore>();
builder.Services.AddScoped<BrowserSessionStorage>();
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthStateProvider>());
builder.Services.AddTransient<AuthorizationHeaderHandler>();

var apiBase = builder.Configuration["ApiBaseUrl"] ?? "http://45.191.0.31:81/api/";
apiBase= "https://localhost:7210/api/";

if (!apiBase.EndsWith('/'))
{
    apiBase += "/";
}
builder.Services.AddHttpClient<ApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBase);
}).AddHttpMessageHandler<AuthorizationHeaderHandler>();

builder.Services.AddScoped<AgentService>();
builder.Services.AddScoped<AgentAduanaService>();
builder.Services.AddScoped<AgentEmbarqueService>();
builder.Services.AddScoped<BookingIncidenceService>();
builder.Services.AddScoped<BookingDtlService>();
builder.Services.AddScoped<BookingETAService>();
builder.Services.AddScoped<BookingPODService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<BlIssuedService>();
builder.Services.AddScoped<CityService>();
builder.Services.AddScoped<CommodityService>();
builder.Services.AddScoped<ContractTypeService>();
builder.Services.AddScoped<ContractService>();
builder.Services.AddScoped<ContractDtlService>();
builder.Services.AddScoped<ContractConditionService>();
builder.Services.AddScoped<ContainerTypeService>();
builder.Services.AddScoped<CostItemService>();
builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<FreeDaysService>();
builder.Services.AddScoped<IncidenceTypeService>();
builder.Services.AddScoped<IncotermService>();
builder.Services.AddScoped<IntermediaryService>();
builder.Services.AddScoped<InvolvedService>();
builder.Services.AddScoped<ItineraryService>();
builder.Services.AddScoped<ItineraryDtlService>();
builder.Services.AddScoped<ItineraryETDService>();
builder.Services.AddScoped<ItineraryTransportCompanyService>();
builder.Services.AddScoped<PaymentConditionService>();
builder.Services.AddScoped<PortService>();
builder.Services.AddScoped<SeasonSerice>();
builder.Services.AddScoped<ServiceService>();
builder.Services.AddScoped<ShipperService>();
builder.Services.AddScoped<ShipperContactService>();
builder.Services.AddScoped<StackingService>();
builder.Services.AddScoped<StatusContainerService>();
builder.Services.AddScoped<TransportCompanyService>();
builder.Services.AddScoped<TransportService>();
builder.Services.AddScoped<TransportCompanyServiceService>();
builder.Services.AddScoped<TransportCompanyItemService>();
builder.Services.AddScoped<TransportCompanyPolService>();
builder.Services.AddScoped<TransportCompanyPodService>();
builder.Services.AddScoped<TtaService>();
builder.Services.AddScoped<TtService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<VesselService>();
builder.Services.AddScoped<WarehouseService>();
builder.Services.AddScoped<ConsigneeService>();
builder.Services.AddScoped<NotifyService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<FilterService>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<InvoiceDetailService>();
builder.Services.AddScoped<PreInvoiceService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<PaymentDetailService>();
builder.Services.AddScoped<BookingLookupService>();

await builder.Build().RunAsync();
