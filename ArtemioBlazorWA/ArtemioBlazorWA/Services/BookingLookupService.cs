using ArtemioBlazorWA.Models;

namespace ArtemioBlazorWA.Services;

public class BookingLookups
{
    public List<AgentModel> Agents { get; set; } = new();
    public List<AgentModel> AgentsEmbarque { get; set; } = new();
    public List<AgentModel> AgentsAduana { get; set; } = new();
    public List<BlIssuedModel> BlIssued { get; set; } = new();
    public List<CommodityModel> Commodities { get; set; } = new();
    public List<ContainerTypeModel> ContainerTypes { get; set; } = new();
    public List<ContractTypeModel> ContractTypes { get; set; } = new();
    public List<IncotermModel> Incoterms { get; set; } = new();
    public List<IntermediaryModel> Intermediaries { get; set; } = new();
    public List<PaymentConditionModel> PaymentConditions { get; set; } = new();
    public List<PortModel> Ports { get; set; } = new();
    public List<ServiceModel> Services { get; set; } = new();
    public List<ShipperModel> Shippers { get; set; } = new();
    public List<TransportModel> Transports { get; set; } = new();
    public List<TransportCompanyModel> TransportCompanies { get; set; } = new();
    public List<VesselModel> Vessels { get; set; } = new();
    public List<WarehouseModel> Warehouses { get; set; } = new();
    public List<ConsigneeModel> Consignees { get; set; } = new();
    public List<NotifyModel> Notifies { get; set; } = new();
    public List<StatusContainerModel> StatusContainers { get; set; } = new();
}

public class BookingLookupService
{
    private readonly AgentService _agents;
    private readonly AgentEmbarqueService _agentsEmbarque;
    private readonly AgentAduanaService _agentsAduana;
    private readonly BlIssuedService _blIssued;
    private readonly CommodityService _commodities;
    private readonly ContainerTypeService _containerTypes;
    private readonly ContractTypeService _contractTypes;
    private readonly IncotermService _incoterms;
    private readonly IntermediaryService _intermediaries;
    private readonly PaymentConditionService _paymentConditions;
    private readonly PortService _ports;
    private readonly ServiceService _services;
    private readonly ShipperService _shippers;
    private readonly TransportService _transports;
    private readonly TransportCompanyService _transportCompanies;
    private readonly VesselService _vessels;
    private readonly WarehouseService _warehouses;
    private readonly ConsigneeService _consignees;
    private readonly NotifyService _notifies;
    private readonly StatusContainerService _statusContainers;

    public BookingLookupService(
        AgentService agents,
        AgentEmbarqueService agentsEmbarque,
        AgentAduanaService agentsAduana,
        BlIssuedService blIssued,
        CommodityService commodities,
        ContainerTypeService containerTypes,
        ContractTypeService contractTypes,
        IncotermService incoterms,
        IntermediaryService intermediaries,
        PaymentConditionService paymentConditions,
        PortService ports,
        ServiceService services,
        ShipperService shippers,
        TransportService transports,
        TransportCompanyService transportCompanies,
        VesselService vessels,
        WarehouseService warehouses,
        ConsigneeService consignees,
        NotifyService notifies,
        StatusContainerService statusContainers)
    {
        _agents = agents;
        _agentsEmbarque = agentsEmbarque;
        _agentsAduana = agentsAduana;
        _blIssued = blIssued;
        _commodities = commodities;
        _containerTypes = containerTypes;
        _contractTypes = contractTypes;
        _incoterms = incoterms;
        _intermediaries = intermediaries;
        _paymentConditions = paymentConditions;
        _ports = ports;
        _services = services;
        _shippers = shippers;
        _transports = transports;
        _transportCompanies = transportCompanies;
        _vessels = vessels;
        _warehouses = warehouses;
        _consignees = consignees;
        _notifies = notifies;
        _statusContainers = statusContainers;
    }

    public async Task<BookingLookups> LoadAsync()
    {
        var lookups = new BookingLookups();
        await Task.WhenAll(
            Assign(() => _agents.GetAllAsync(), x => lookups.Agents = x),
            Assign(() => _agentsEmbarque.GetAllAsync(), x => lookups.AgentsEmbarque = x),
            Assign(() => _agentsAduana.GetAllAsync(), x => lookups.AgentsAduana = x),
            Assign(() => _blIssued.GetAllAsync(), x => lookups.BlIssued = x),
            Assign(() => _commodities.GetAllAsync(), x => lookups.Commodities = x),
            Assign(() => _containerTypes.GetAllAsync(), x => lookups.ContainerTypes = x),
            Assign(() => _contractTypes.GetAllAsync(), x => lookups.ContractTypes = x),
            Assign(() => _incoterms.GetAllAsync(), x => lookups.Incoterms = x),
            Assign(() => _intermediaries.GetAllAsync(), x => lookups.Intermediaries = x),
            Assign(() => _paymentConditions.GetAllAsync(), x => lookups.PaymentConditions = x),
            Assign(() => _ports.GetAllAsync(), x => lookups.Ports = x),
            Assign(() => _services.GetAllAsync(), x => lookups.Services = x),
            Assign(() => _shippers.GetAllAsync(), x => lookups.Shippers = x),
            Assign(() => _transports.GetAllAsync(), x => lookups.Transports = x),
            Assign(() => _transportCompanies.GetAllAsync(), x => lookups.TransportCompanies = x),
            Assign(() => _vessels.GetAllAsync(), x => lookups.Vessels = x),
            Assign(() => _warehouses.GetAllAsync(), x => lookups.Warehouses = x),
            Assign(() => _consignees.GetAllAsync(), x => lookups.Consignees = x),
            Assign(() => _notifies.GetAllAsync(), x => lookups.Notifies = x),
            Assign(() => _statusContainers.GetAllAsync(), x => lookups.StatusContainers = x)
        );
        return lookups;
    }

    private static async Task Assign<T>(Func<Task<List<T>>> load, Action<List<T>> set)
    {
        set(await load() ?? new List<T>());
    }
}
