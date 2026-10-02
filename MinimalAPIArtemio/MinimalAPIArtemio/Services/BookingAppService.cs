using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;

namespace MinimalAPIArtemio.Services;

public class BookingAppService
{
    public Task<List<BookingModel>> GetAllAsync()
    {
        var db = new ClaseDB<BookingModel>();
        return db.GetAllAsync("SELECT * FROM Booking;");
    }

    public Task<BookingModel?> GetAsync(int id)
    {
        var db = new ClaseDB<BookingModel>();
        return db.GetAsync("SELECT * FROM Booking WHERE BookingId = @id;", new { id });
    }

    public async Task<int> CreateAsync(BookingModel data)
    {
        var db = new ClaseDB<BookingModel>();
        return await db.ExecuteInsertAsync(data, @"INSERT INTO Booking
            (SeasonId, BookingDate, SendEmailDate, UploadDate, BookingStatus, ContractCo, TransportCompanyId, VesselId, Voyage, ServiceId, POLId, PODId, DeliveryPlace, ShipperId, ConsigneeId, NotifyId, AgentId, CommodityId, IncotermId, PaymentConditionId, ContractTypeId, IntermediaryId, WarehouseId, PoRef, NroBooking, HsCode, LastETD, StackingEmail)
            VALUES
            (@SeasonId, @BookingDate, @SendEmailDate, @UploadDate, @BookingStatus, @ContractCo, @TransportCompanyId, @VesselId, @Voyage, @ServiceId, @POLId, @PODId, @DeliveryPlace, @ShipperId, @ConsigneeId, @NotifyId, @AgentId, @CommodityId, @IncotermId, @PaymentConditionId, @ContractTypeId, @IntermediaryId, @WarehouseId, @PoRef, @NroBooking, @HsCode, @LastETD, @StackingEmail);");
    }
}
