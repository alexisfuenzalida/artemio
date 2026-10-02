using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using MinimalAPIArtemio.Database;
using MinimalAPIArtemio.Helpers;
using MinimalAPIArtemio.Models;
using MinimalAPIArtemio.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MinimalAPIArtemio.Endpoints;

public static class ReportEndpoints
{
    public static void MapReportEndpoints(this IEndpointRouteBuilder app)
    {
app.MapPost("/api/contractsfilters", async (ContractFilterModel filter) =>
{
    try
    {
        var cDb = new ClaseDB<ContractReportModel>();

        var q = "";
        var w = new List<string>();

        if(filter.TransportCompanyId > 0)
        {
            w.Add($" c.TransportCompanyId = {filter.TransportCompanyId} ");
        }
        if (filter.ServiceId > 0)
        {
            w.Add($" c.ServiceId = {filter.ServiceId} ");
        }
        if (filter.PODId > 0)
        {
            w.Add($" pod.PortId = {filter.PODId} ");
        }
        if (filter.POLId > 0)
        {
            w.Add($" pol.PortId = {filter.POLId} ");
        }

        q = (w.Count > 0 ? $" WHERE { string.Join(" AND ", w.ToArray()) }" : "");

        var data = await cDb.GetAllAsync(@$"select
	                                            c.`ContractId`,
	                                            tc.`TransportCompany`,
	                                            c.`NroContract`,
	                                            c.`From`,
	                                            c.`Until`,
	                                            co.`Commodity`,
	                                            pol.`Port` as POL,
	                                            c.`POT1`,
	                                            c.`POT2`,
	                                            c.`POT3`,
	                                            pod.`Port` as POD,
                                                con.Country,
	                                            c.`TT`,
	                                            c.`FreeDaysOrigin`,
	                                            c.`FreeDaysDestination`,
	                                            c.`Public`,
	                                            c.`VolumeDiscount`,
	                                            c.`Net`,
                                                ifnull((select sum(Amount) from ContractsDetails where ContractId = c.ContractId and Include = 'Not Include'),0) as TotalNotInclude,
	                                            c.`FreeArtemio`,
	                                            se.`Service`,
	                                            c.`Comment`,
	                                            sh.`Shipper`,
	                                            c.`Status`
                                            from Contracts c
                                            left join TransportCompanies tc on tc.`TransportCompanyId` = c.`TransportCompanyId`
                                            left join Commodities co on co.`CommodityId` = c.`CommodityId`
                                            left join Ports pol on pol.`PortId` = c.`POLId`
                                            left join Ports pod on pod.`PortId` = c.`PODId`
                                            left join Countries con on con.CountryId = pod.CountryId
                                            left join Services se on se.`ServiceId` = c.`ServiceId`
                                            left join Shippers sh on sh.`ShipperId` = c.`ShipperId` {q};");

        if (data.Count() == 0) return Results.NotFound(new
        {
            Message = "Not Found"
        });

        var ids = data.Select(k => k.ContractId).ToArray();

        var cDbDetail = new ClaseDB<ContractDetailReportModel>();
        var dataDetail = await cDbDetail.GetAllAsync(@$"SELECT
	                                                ci.`CostItem`,
	                                                cd.`Amount`,
	                                                cd.`Include`,
	                                                cd.`ContractId`
                                                FROM `ContractsDetails` cd
                                                LEFT JOIN CostItems ci ON ci.`CostItemId` = cd.`CostItemId`
                                                WHERE cd.ContractId IN ({ string.Join(",",ids) });");

        var Items = dataDetail.GroupBy(k => k.CostItem).ToList();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            dir.Create();
        }

        var file = new FileInfo(@$"{dir.FullName}\Contract Report.xlsx");

        if (file.Exists)
        {
            file.Delete();
        }

        using (var package = new ExcelPackage(file))
        {

            ExcelWorksheet sheet1;

            if (package.Workbook.Worksheets.Count > 0)
            {
                sheet1 = package.Workbook.Worksheets.First();
            }
            else
            {
                sheet1 = package.Workbook.Worksheets.Add("Report");
            }

            sheet1.Cells[1, 1].Value = "Compañia";
            sheet1.Cells[1, 2].Value = "N°CONTRATO";
            sheet1.Cells[1, 3].Value = "Vigencia Desde";
            sheet1.Cells[1, 4].Value = "Vigencia Hasta";
            sheet1.Cells[1, 5].Value = "COMMODITY";
            sheet1.Cells[1, 6].Value = "POL";
            sheet1.Cells[1, 7].Value = "POT1";
            sheet1.Cells[1, 8].Value = "POT2";
            sheet1.Cells[1, 9].Value = "POT3";
            sheet1.Cells[1, 10].Value = "POD";
            sheet1.Cells[1, 11].Value = "Country";
            sheet1.Cells[1, 12].Value = "Ttime";
            sheet1.Cells[1, 13].Value = "FreeDaysOrigin";
            sheet1.Cells[1, 14].Value = "FreeDaysDestination";
            sheet1.Cells[1, 15].Value = "PUBLIC FREIGHT";
            sheet1.Cells[1, 16].Value = "VOLUME DISCOUNT";
            sheet1.Cells[1, 17].Value = "NET FREIGHT";
            sheet1.Cells[1, 18].Value = "TotalNotInclude";
            sheet1.Cells[1, 19].Value = "Fee Artemio";
            sheet1.Cells[1, 20].Value = "Service";

            var row = 0;
            var colItems = 21;

            while (row < Items.Count())
            {
                sheet1.Cells[1, (row + colItems)].Value = Items[row].Key;

                row++;
            }


            var fil = 2;
            foreach (var item in data)
            {
                sheet1.Cells[fil, 1].Value = item.TransportCompany;
                sheet1.Cells[fil, 2].Value = item.NroContract;
                sheet1.Cells[fil, 3].Value = item.From;
                sheet1.Cells[fil, 3].Style.Numberformat.Format = "dd/MM/yyyy";
                sheet1.Cells[fil, 4].Value = item.Until;
                sheet1.Cells[fil, 4].Style.Numberformat.Format = "dd/MM/yyyy";
                sheet1.Cells[fil, 5].Value = item.Commodity;
                sheet1.Cells[fil, 6].Value = item.POL;
                sheet1.Cells[fil, 7].Value = item.POT1;
                sheet1.Cells[fil, 8].Value = item.POT2;
                sheet1.Cells[fil, 9].Value = item.POT3;
                sheet1.Cells[fil, 10].Value = item.POD;
                sheet1.Cells[fil, 11].Value = item.Country;
                sheet1.Cells[fil, 12].Value = item.TT;
                sheet1.Cells[fil, 13].Value = item.FreeDaysOrigin;
                sheet1.Cells[fil, 14].Value = item.FreeDaysDestination;
                sheet1.Cells[fil, 15].Value = item.Public;
                sheet1.Cells[fil, 16].Value = item.VolumeDiscount;
                sheet1.Cells[fil, 17].Value = item.Net;
                sheet1.Cells[fil, 18].Value = item.TotalNotInclude;
                sheet1.Cells[fil, 19].Value = item.FreeArtemio;
                sheet1.Cells[fil, 20].Value = item.Service;

                var lstDtl = dataDetail.Where(i => i.ContractId == item.ContractId).ToList();

                foreach (var dtl in lstDtl)
                {
                    var i = 0;
                    while (i < Items.Count)
                    {
                        if (dtl.CostItem == Items[i].Key)
                        {
                            sheet1.Cells[fil, (colItems + i)].Value = dtl.Amount;
                        }

                        i++;
                    }
                }

                fil++;
            }


            await package.SaveAsync();

            var stream = new FileStream(file.FullName, FileMode.Open);

            //return Results.File(stream,"application/octet-stream", file.FullName);
            return Results.File(stream, fileDownloadName: file.Name);

        }
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});

app.MapPost("/api/itinerariesfilters", async (ItineraryFilterModel filter) =>
{
    try
    {
        var cDb = new ClaseDB<ItineraryReportModel>();

        var q = "";
        var w = new List<string>();

        if (filter.TransportCompanyId > 0)
        {
            w.Add($" i.OperatorId = {filter.TransportCompanyId} ");
        }
        if (filter.ServiceId > 0)
        {
            w.Add($" i.ServiceId = {filter.ServiceId} ");
        }
        if (filter.POLId > 0)
        {
            w.Add($" pol.PortId = {filter.POLId} ");
        }
        if (filter.PODId.Length > 0)
        {
            w.Add($" i.ItineraryId IN (SELECT it.ItineraryId FROM ItinerariesDetails it WHERE it.PODId IN ({string.Join(",", filter.PODId.ToArray())})) ");
        }
        if (filter.From != null && filter.To != null)
        {
            w.Add($" i.ETD BETWEEN '{filter.From.Value.ToString("yyyy/MM/dd")}' AND '{filter.To.Value.ToString("yyyy/MM/dd")}' ");
        }
        
        q = (w.Count > 0 ? $" WHERE {string.Join(" AND ", w.ToArray())}" : "");

        var sSql = @$"select
                                                i.ItineraryId,
	                                            i.`Week`,
	                                            ifnull(v.`Vessel`,'') as Vessel,
	                                            ifnull(s.`Service`,'') as Service,
	                                            ifnull(tc.`TransportCompany`,'') as Operator,
	                                            (select group_concat(TransportCompany) from TransportCompanies where TransportCompanyId in (select itc.TransportCompanyId from ItinerariesTransportCompanies itc where itc.ItineraryId = i.ItineraryId)) as 'Companies',
	                                            pol.Port as POL,
	                                            IFNULL(i.`Stacking`,'') AS Stacking,
	                                            IFNULL(i.`LateArrival`,'') AS LateArrival 
                                            from `Itineraries` i
                                            left join Ports pol on pol.`PortId` = i.`POLId`
                                            left join Vessels v on v.`VesselId` = i.`VesselId`
                                            left join Services s on s.`ServiceId` = i.`ServiceId`
                                            left join TransportCompanies tc on tc.`TransportCompanyId` = i.`OperatorId` {q}
                                            group by i.ItineraryId,i.`Week`,i.`VesselId`,i.ServiceId,i.`SeasonId`,i.`OperatorId`
                                            order by i.ETD,s.Service;";

        var data = await cDb.GetAllAsync(sSql);

        if (data.Count == 0)
        {

            return Results.NotFound(new
            {
                Message = "Not Found"
            });
        }

        var ids = data.Select(k => k.ItineraryId).ToArray();
        var idsArr = string.Join(",", ids);
        var podArr = string.Join(",", filter.PODId.ToArray());

        var cDbETD = new ClaseDB<ItineraryReportETDModel>();
        var cDbETA = new ClaseDB<ItineraryReportETAModel>();

        var dataETD = await cDbETD.GetAllAsync(@$"select 
                                                        pol.Port as POL,
                                                        i.ETD,
                                                        i.ItineraryId
                                                from ItinerariesETD i 
                                                inner join Ports pol on pol.PortId = i.POLId WHERE i.ItineraryId IN ({idsArr});");

        var POLs = dataETD.GroupBy(k => k.POL).ToList();

        if (filter.PODId.Length > 0)
        {
            q = $" AND i.PODId IN ({podArr}) ";
        }
        else
        {
            q = "";
        }
        sSql = @$"select 
                                                        pod.Port as POD,
                                                        i.ETA,
                                                        i.ItineraryId
                                                from ItinerariesDetails i 
                                                left join Ports pod on pod.PortId = i.PODId where i.ItineraryId in ({idsArr}) {q}";
        
        var dataETA = await cDbETA.GetAllAsync(sSql);

        var PODs = dataETA.GroupBy(k => k.POD).ToList();


        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            dir.Create();
        }

        var file = new FileInfo(@$"{dir.FullName}\Itinerary Report.xlsx");
        
        if (file.Exists)
        {
            file.Delete();
        }

        using (var package = new ExcelPackage(file))
        {

            ExcelWorksheet sheet1;

            if (package.Workbook.Worksheets.Count > 0)
            {
                sheet1 = package.Workbook.Worksheets.First();
            }
            else
            {
                sheet1 = package.Workbook.Worksheets.Add("Report");
            }

            var logo = new FileInfo($"{Path.Combine("Images","artemio.png")}");

            var excelImage2 = sheet1.Drawings.AddPicture("Artemio", logo);
            //excelImage2.SetSize(70);

            excelImage2.SetPosition(1, 0, 1, 0);

            sheet1.Cells[5, 1].Value = "WEEK";
            sheet1.Cells[5, 2].Value = "SERVICIO";
            sheet1.Cells[5, 3].Value = "NAVE";
            sheet1.Cells[5, 4].Value = "OPERADOR";
            sheet1.Cells[5, 5].Value = "NAVIERAS";
            sheet1.Cells[5, 6].Value = "STACKING";
            sheet1.Cells[5, 7].Value = "LATE ARRIVAL";

            sheet1.Rows[5].Height = 60.00;

            var row = 0;
            var colETD = 8;

            while (row < POLs.Count())
            {
                sheet1.Cells[5, (row + colETD)].Value = POLs[row].Key;
                //sheet1.Cells[5, (row + colETD + 1)].Value = $"Stacking {POLs[row].Key}";
                //sheet1.Cells[5, (row + colETD)].Value = POLs[row].Key;

                row++;
            }

            row = 0;
            var colETA = colETD + POLs.Count();

            while (row < PODs.Count())
            {
                sheet1.Cells[5, (row + colETA)].Value = PODs[row].Key;

                row++;
            }

            var lastCol = 5 + POLs.Count() + PODs.Count();

            var fil = 6;
            foreach(var item in data)
            {
                sheet1.Cells[fil, 1].Value = item.Week;
                sheet1.Cells[fil, 2].Value = item.Service;
                
                if(item.POL != null)
                {
                    sheet1.Cells[fil, 3].Value = item.Vessel;
                    sheet1.Cells[fil, 4].Value = item.Operator;
                    sheet1.Cells[fil, 5].Value = item.Companies.Replace(","," - ");
                    sheet1.Cells[fil, 6].Value = item.Stacking;
                    sheet1.Cells[fil, 7].Value = item.LateArrival;

                    var lstETD = dataETD.Where(i => i.ItineraryId == item.ItineraryId).ToList();
                
                    foreach(var pol in lstETD)
                    {
                        var i = 0;

                        while (i < POLs.Count)
                        {
                            if (pol.POL == POLs[i].Key)
                            {
                                sheet1.Cells[fil, (colETD+i)].Value = pol.ETD;
                                sheet1.Cells[fil, (colETD+i)].Style.Numberformat.Format = "dd/MM/yyyy";
                            }

                            i++;
                        }
                    }

                    var lstETA = dataETA.Where(i => i.ItineraryId == item.ItineraryId).ToList();

                    foreach(var pod in lstETA)
                    {
                        var i = 0;
                        while(i< PODs.Count)
                        {
                            if (pod.POD == PODs[i].Key)
                            {
                                if(pod.ETA == null)
                                {
                                    sheet1.Cells[fil, (colETA + i)].Value = "No Data";

                                    sheet1.Cells[fil, (colETA + i)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    sheet1.Cells[fil, (colETA + i)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                }
                                else
                                {
                                    sheet1.Cells[fil, (colETA + i)].Value = pod.ETA;
                                    sheet1.Cells[fil, (colETA + i)].Style.Numberformat.Format = "dd/MM/yyyy";
                                }
                            }

                            i++;
                        }
                    }

                }
                else
                {
                    var add = sheet1.Cells[fil, lastCol].Address;

                    sheet1.Cells[$"C{ fil }:{ add }"].Merge = true;
                    sheet1.Cells[$"C{fil}:{add}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    sheet1.Cells[$"C{fil}:{add}"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    sheet1.Cells[fil, 3].Value = "BLANK SAILING";

                }
                
                fil++;
            }

            sheet1.Columns.AutoFit();

            await package.SaveAsync();

            var stream = new FileStream(file.FullName, FileMode.Open);

            //return Results.File(stream,"application/octet-stream", file.FullName);
            return Results.File(stream, fileDownloadName: file.Name);

        }

    }
    catch (Exception ex)
    {

        Console.WriteLine(ex.Message);

        return Results.BadRequest(new
        {
            Message = "Error"
        });
    }
});
    }
}
