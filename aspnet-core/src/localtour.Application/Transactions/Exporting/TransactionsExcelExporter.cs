using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using localtour.DataExporting.Excel.EpPlus;
using localtour.Storage;
using localtour.Transactions.Dto;
using System.Collections.Generic;

namespace localtour.Transactions.Exporting
{
    public class TransactionsExcelExporter : EpPlusExcelExporterBase, ITransactionsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TransactionsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTransactionForViewDto> bookings)
        {
            return CreateExcelPackage(
                "Transactions.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Transactions"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        "Transaction Code",
                        "Booking Code",
                        "Tour Name",
                        "Name On Card",
                        "Transaction Date",
                        "Status",
                        "Amount"
                        );

                    AddObjects(
                        sheet, 2, bookings,
                        _ => "T-" + _.Transaction.Id,
                        _ => "B-" + _.Transaction.BookingId,
                        _ => _.TourName,
                        _ => _.Transaction.NameOnCard,
                        _ => _.Transaction.TransactionDate,
                        _ => _.Transaction.Status,
                        _ => _.Transaction.Amount
                        );
                });
        }
    }
}
