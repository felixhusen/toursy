using System.Collections.Generic;
using localtour.Bookings.Dto;
using localtour.DataExporting.Excel.EpPlus;

namespace localtour.Bookings.Exporting
{
    public interface IBookingsExcelExporter
    {
        FileDto ExportToFile(List<GetBookingForViewDto> assets);
    }
}