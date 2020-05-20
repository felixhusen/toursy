using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Disputes.Dto;
using localtour.Storage;
using System.Collections.Generic;

namespace localtour.Disputes.Exporting
{
    public class DisputesExcelExporter : EpPlusExcelExporterBase, IDisputesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DisputesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDisputeForViewDto> bookings)
        {
            return CreateExcelPackage(
                "Disputes.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Disputes"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "Dispute Code",
                        "Booking Code",
                        "Tour Name",
                        "User Name",
                        "Description",
                        "Date",
                        "Status"
                        );

                    AddObjects(
                        sheet, 2, bookings,
                        _ => "D-" + _.Dispute.Id,
                        _ => _.BookingCode,
                        _ => _.TourName,
                        _ => _.UserFullName,
                        _ => _.Dispute.Description,
                        _ => _.Dispute.Date,
                        _ => _.Dispute.Status
                        );
                });
        }
    }
}
