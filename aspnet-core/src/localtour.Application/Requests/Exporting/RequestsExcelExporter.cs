using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Requests.Dto;
using localtour.Storage;
using System.Collections.Generic;

namespace localtour.Requests.Exporting
{
    public class RequestsExcelExporter : EpPlusExcelExporterBase, IRequestsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RequestsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRequestForViewDto> bookings)
        {
            return CreateExcelPackage(
                "Requests.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Requests"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "Request Code",
                        "Booking Code",
                        "Tour Name",
                        "User Name",
                        "Description",
                        "Date",
                        "Status"
                        );

                    AddObjects(
                        sheet, 2, bookings,
                        _ => "R-" + _.Request.Id,
                        _ => _.BookingCode,
                        _ => _.TourName,
                        _ => _.UserFullName,
                        _ => _.Request.Description,
                        _ => _.Request.Date,
                        _ => _.Request.Status
                        );
                });
        }
    }
}
