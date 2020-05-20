using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using localtour.Bookings.Dto;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Storage;
using System.Collections.Generic;

namespace localtour.Bookings.Exporting
{
    public class BookingsExcelExporter : EpPlusExcelExporterBase, IBookingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BookingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBookingForViewDto> bookings)
        {
            return CreateExcelPackage(
                "Bookings.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Bookings"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "Booking Code",
                        "Tour Name",
                        "Name",
                        "Phone Number",
                        "Number Of People",
                        "Status",
                        "Total Price"
                        );

                    AddObjects(
                        sheet, 2, bookings,
                        _ => "B-" + _.Booking.Id,
                        _ => _.TourName,
                        _ => _.Booking.Name,
                        _ => _.Booking.PhoneNumber,
                        _ => _.Booking.NumberOfPeople,
                        _ => _.Booking.Status,
                        _ => _.Booking.TotalPrice
                        );
                });
        }
    }
}
