using Abp.Collections.Extensions;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Storage;
using localtour.Tours.Dto;
using System.Collections.Generic;
using System.Linq;

namespace localtour.Tours.Exporting
{
    public class ToursExcelExporter : EpPlusExcelExporterBase, IToursExcelExporter
    {
        public ToursExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) {}

        public FileDto ExportToFile(List<GetTourForViewDto> bookings)
        {
            return CreateExcelPackage(
                "Tours.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Tours"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "#",
                        "Tour Name",
                        "Tour Date",
                        "Description"
                        );

                    AddObjects(
                        sheet, 2, bookings,
                        _ => _.Tour.Id,
                        _ => _.Tour.Name,
                        _ => _.TourDates.Select(t => t.StartDate.ToString("dd/MM/yyyy")).JoinAsString(","),
                        _ => _.Tour.Description
                        );
                });
        }
    }
}
