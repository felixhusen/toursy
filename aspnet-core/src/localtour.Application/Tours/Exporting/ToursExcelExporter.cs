using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Tours.Dto;
using localtour.Storage;
using System.Collections.Generic;
using System.Linq;
using Abp.Collections.Extensions;

namespace localtour.Tours.Exporting
{
    public class ToursExcelExporter : EpPlusExcelExporterBase, IToursExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ToursExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

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
