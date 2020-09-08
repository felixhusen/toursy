using System.Collections.Generic;
using localtour.Requests.Dto;
using localtour.DataExporting.Excel.EpPlus;

namespace localtour.Requests.Exporting
{
    public interface IRequestsExcelExporter
    {
        FileDto ExportToFile(List<GetRequestForViewDto> assets);
    }
}