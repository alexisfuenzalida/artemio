using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Models;
using OfficeOpenXml;

namespace MinimalAPIArtemio.Endpoints;

public static class ItineraryImportEndpoints
{
    public static void MapItineraryImportEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/itinerariesimport", async (IFormFile file, [FromForm] int seasonId) =>
        {
            if (file == null || file.Length == 0)
            {
                return Results.BadRequest(new { Message = "Debes seleccionar un archivo." });
            }

            if (!Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return Results.BadRequest(new { Message = "El archivo debe ser un Excel (.xlsx)." });
            }

            try
            {
                var cDbPort = new ClaseDB<PortModel>();
                var ports = await cDbPort.GetAllAsync("SELECT * FROM Ports;");

                var cDbVessel = new ClaseDB<VesselModel>();
                var vessels = await cDbVessel.GetAllAsync("SELECT * FROM Vessels;");

                var cDbService = new ClaseDB<ServiceModel>();
                var services = await cDbService.GetAllAsync("SELECT * FROM Services;");

                var cDbTransportCompany = new ClaseDB<TransportCompanyModel>();
                var transportCompanies = await cDbTransportCompany.GetAllAsync("SELECT * FROM TransportCompanies;");

                var cDbItinerary = new ClaseDB<ItineraryModel>();
                var existingItineraries = await cDbItinerary.GetAllAsync("SELECT * FROM Itineraries;");

                var wanHai = transportCompanies.FirstOrDefault(x => string.Equals(x.TransportCompany?.Trim(), "Wan Hai", StringComparison.OrdinalIgnoreCase));
                if (wanHai == null)
                {
                    return Results.BadRequest(new { Message = "No existe la naviera 'Wan Hai' en TransportCompanies." });
                }

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets["Captura Semanal"];
                if (worksheet == null || worksheet.Dimension == null)
                {
                    return Results.BadRequest(new { Message = "No se encontro la hoja 'Captura Semanal' en el archivo." });
                }

                var result = new ItineraryImportResultModel();
                var cDbEtd = new ClaseDB<ItineraryETDModel>();
                var cDbDetail = new ClaseDB<ItineraryDetailModel>();
                var cDbItineraryTc = new ClaseDB<ItineraryTransportCompanyModel>();

                string GetText(int row, int col) => (worksheet.Cells[row, col].Text ?? "").Trim();

                DateTime? GetDate(int row, int col)
                {
                    var value = worksheet.Cells[row, col].GetValue<DateTime?>();
                    if (value.HasValue)
                    {
                        return value;
                    }
                    var text = GetText(row, col);
                    return DateTime.TryParse(text, out var parsed) ? parsed : (DateTime?)null;
                }

                var lastRow = worksheet.Dimension.End.Row;
                for (var row = 3; row <= lastRow; row++)
                {
                    var polText = GetText(row, 1);
                    var podText = GetText(row, 2);
                    var serviceText = GetText(row, 3);
                    var ttText = GetText(row, 5);
                    var etd = GetDate(row, 7);
                    var vesselText = GetText(row, 8);
                    var voyageText = GetText(row, 9);
                    var etaReal = GetDate(row, 11);

                    if (etd == null || string.IsNullOrWhiteSpace(vesselText) || string.IsNullOrWhiteSpace(voyageText))
                    {
                        continue;
                    }

                    result.TotalRows++;

                    var pol = ports.FirstOrDefault(x => string.Equals(x.Port?.Trim(), polText, StringComparison.OrdinalIgnoreCase));
                    if (pol == null)
                    {
                        result.Errors.Add(new ItineraryImportRowError { Row = row, Message = $"POL '{polText}' no existe." });
                        continue;
                    }

                    var pod = ports.FirstOrDefault(x => string.Equals(x.Port?.Trim(), podText, StringComparison.OrdinalIgnoreCase));
                    if (pod == null)
                    {
                        result.Errors.Add(new ItineraryImportRowError { Row = row, Message = $"POD '{podText}' no existe." });
                        continue;
                    }

                    var vessel = vessels.FirstOrDefault(x => string.Equals(x.Vessel?.Trim(), vesselText, StringComparison.OrdinalIgnoreCase));
                    if (vessel == null)
                    {
                        result.Errors.Add(new ItineraryImportRowError { Row = row, Message = $"Vessel '{vesselText}' no existe." });
                        continue;
                    }

                    var service = services.FirstOrDefault(x => string.Equals(x.Service?.Trim(), serviceText, StringComparison.OrdinalIgnoreCase));
                    if (service == null)
                    {
                        result.Errors.Add(new ItineraryImportRowError { Row = row, Message = $"Service '{serviceText}' no existe." });
                        continue;
                    }

                    var isDuplicate = existingItineraries.Any(x =>
                        x.POLId == pol.PortId &&
                        x.VesselId == vessel.VesselId &&
                        string.Equals(x.Voyage?.Trim(), voyageText, StringComparison.OrdinalIgnoreCase) &&
                        x.ETD.Date == etd.Value.Date);

                    if (isDuplicate)
                    {
                        result.Skipped++;
                        continue;
                    }

                    try
                    {
                        var tt = int.TryParse(ttText, out var ttParsed) ? ttParsed : 0;
                        var eta = etaReal ?? etd.Value.AddDays(tt);
                        var week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(etd.Value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

                        var itinerary = new ItineraryModel
                        {
                            Week = week,
                            ETD = etd.Value,
                            POLId = pol.PortId,
                            VesselId = vessel.VesselId,
                            ServiceId = service.ServiceId,
                            OperatorId = wanHai.TransportCompanyId,
                            SeasonId = seasonId,
                            Voyage = voyageText,
                            Stacking = "",
                            LateArrival = "",
                            ExtraLateArrival = "",
                            CutOff = ""
                        };

                        itinerary.ItineraryId = await cDbItinerary.ExecuteInsertAsync(itinerary,
                            "INSERT INTO Itineraries (Week,ETD,POLId,VesselId,ServiceId,OperatorId,SeasonId,Voyage,Stacking,LateArrival,ExtraLateArrival,CutOff) VALUES (@Week,@ETD,@POLId,@VesselId,@ServiceId,@OperatorId,@SeasonId,@Voyage,@Stacking,@LateArrival,@ExtraLateArrival,@CutOff);");

                        await cDbEtd.ExcecuteQueryAsync(
                            new ItineraryETDModel { POLId = pol.PortId, ETD = etd.Value, IsMain = true, ItineraryId = itinerary.ItineraryId },
                            "INSERT INTO ItinerariesETD (POLId,ETD,IsMain,ItineraryId) VALUES (@POLId,@ETD,@IsMain,@ItineraryId);");

                        await cDbDetail.ExcecuteQueryAsync(
                            new ItineraryDetailModel { PODId = pod.PortId, ETA = eta, TT = tt, ItineraryId = itinerary.ItineraryId },
                            "INSERT INTO ItinerariesDetails (PODId,ETA,TT,ItineraryId) VALUES (@PODId,@ETA,@TT,@ItineraryId);");

                        await cDbItineraryTc.ExcecuteQueryAsync(
                            new ItineraryTransportCompanyModel { TransportCompanyId = wanHai.TransportCompanyId, ItineraryId = itinerary.ItineraryId },
                            "INSERT INTO ItinerariesTransportCompanies (TransportCompanyId,ItineraryId) VALUES (@TransportCompanyId,@ItineraryId);");

                        existingItineraries.Add(itinerary);
                        result.Imported++;
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add(new ItineraryImportRowError { Row = row, Message = $"Error al insertar: {ex.Message}" });
                    }
                }

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { Message = $"No se pudo procesar el archivo: {ex.Message}" });
            }
        }).DisableAntiforgery();
    }
}
